
using AutoMapper;
using Casino.API.Services;
using Casino.Data.Context;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.API.Components
{
    public abstract class CRUDComponent<T> where T : class
    {
        public IIdentityApp<User> IdentityApp { get; }
        public IMapper Mapper { get; }
        public ContextCRUD<T> ContextCRUD { get; }

        public abstract Type ShowModelDTOType { get; internal set; }

        public CRUDComponent(
            ApplicationDbContext dbContext,
            ContextCRUD<T> contextCRUD,
            IIdentityApp<User> identityApp,
            IMapper mapper)
        {
            IdentityApp = identityApp;
            Mapper = mapper;
            ContextCRUD = contextCRUD;

            ContextCRUD.AppDbContext = dbContext;
        }

        #region Find functions

        public virtual async Task<ActionResult<WebApiResponse>> FindAllAndResponseAsync(int page, int recordsPerPage)
        {
            IPagedRecords pagedRecords = await ContextCRUD.FindAllPagedAsync(page, recordsPerPage);

            pagedRecords = MapPagedRecordsToModelDTO(pagedRecords);

            return MakeSuccessResponse(pagedRecords, "");
        }

        public virtual async Task<ActionResult<WebApiResponse>> FindFromIdAndResponseAsync(long id)
        {
            T entity = await ContextCRUD.FindByIdAsync(id);

            return MapEntityAndMakeSuccessResponse(entity);
        }

        #endregion

        #region Create functions

        public virtual async Task<ActionResult<WebApiResponse>> CreateFromModelDTOAndResponseAsync(IModelDTO modelDTO)
        {
            T entity = Activator.CreateInstance<T>();

            entity = await FillEntityFromDTO(entity, modelDTO);

            return await CreateFromEntityAndResponseAsync(entity);
        }

        public virtual async Task<ActionResult<WebApiResponse>> CreateFromEntityAndResponseAsync(T entity)
        {
            await SetIdentityUserToEntity(entity);

            entity = await ContextCRUD.CreateFromEntityAsync(entity);

            return MapEntityAndMakeSuccessResponse(entity);
        }        

        #endregion

        #region Update functions

        public virtual async Task<ActionResult<WebApiResponse>> UpdateFromModelDTOAndResponseAsync(long id, IModelDTO modelDTO)
        {
            T entity = await ContextCRUD.FindByIdAsync(id);

            entity = await FillEntityFromDTO(entity, modelDTO);

            entity = SetEntityId((IEntityModelBase)entity, id);

            return await UpdateFromEntityAndResponseAsync(entity);
        }

        protected virtual T SetEntityId(IEntityModelBase entity, long id)
        {
            entity.Id = id;

            return (T)entity;
        }

        public virtual async Task<ActionResult<WebApiResponse>> UpdateFromEntityAndResponseAsync(T entity)
        {
            entity = await ContextCRUD.UpdateFromEntityAsync(entity);

            return MapEntityAndMakeSuccessResponse(entity);
        }

        #endregion

        #region Delete function

        public virtual async Task<ActionResult<WebApiResponse>> DeleteFromIdAndResponseAsync(long id)
        {
            T entity = await ContextCRUD.FindByIdAsync(id);

            return await DeleteFromEntityAndResponseAsync(entity);
        }

        public virtual async Task<ActionResult<WebApiResponse>> DeleteFromEntityAndResponseAsync(T entity)
        {
            entity = await ContextCRUD.DeleteByEntityAsync(entity);

            return MapEntityAndMakeSuccessResponse(entity);
        }

        #endregion


        #region Helper functions

        public abstract Task<T> FillEntityFromDTO(T entity, IModelDTO modelDTO);

        public virtual IModelDTO MapEntityToModelDTO(T entity)
        {
            return (IModelDTO)Mapper.Map(entity, typeof(T), ShowModelDTOType);
        }

        public abstract IPagedRecords MapPagedRecordsToModelDTO(IPagedRecords pagedRecords);

        public virtual async Task<T> SetIdentityUserToEntity(T entity)
        {
            return await Task.Run(() => entity);
        }
    

        public virtual ActionResult<WebApiResponse> MapEntityAndMakeSuccessResponse(T entity, string message = "success!")
        {
            var map = MapEntityToModelDTO(entity);

            return MakeSuccessResponse(map, message);
        }

        public virtual ActionResult<WebApiResponse> MakeSuccessResponse(object data, string message)
        {
            return new WebApiResponse()
                .Success()
                .SetData(data)
                .SetMessage(message);
        }

        #endregion
    }
}
