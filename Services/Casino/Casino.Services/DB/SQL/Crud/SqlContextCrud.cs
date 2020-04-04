using AutoMapper;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.DB.SQL.Queryable;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Services.DB.SQL.Crud
{
    public class SqlContextCrud<T> : ISqlContextCrud<T> where T : class
    {
        private readonly IMapper _mapper = null;
        private readonly IPagedRecords<T> _pagedRecords = null;

        protected string EntityClassName => typeof(T).Name;

        protected IPagedRecords<T> PagedRecords => _pagedRecords;

        public DbContext AppDbContext { get; set; }
        public IMapper Mapper => _mapper;
        public IQueryableFilter<T> QueryFilter { get; set; }
        protected Type ShowModelDTOType { get; set; }

        public SqlContextCrud(IMapper mapper, IPagedRecords<T> pagedRecords)
        {
            _mapper = mapper;
            _pagedRecords = pagedRecords;

            QueryFilter = new QueryableDefaultFilter<T>();
        }

        #region Queryables section

        public virtual IQueryable<T> GetQueryableWithFilter()
        {
            return QueryFilter.SetFilter(GetQueryableWithoutFilter());
        }

        public virtual IQueryable<T> GetQueryableWithoutFilter()
        {
            return AppDbContext.Set<T>();
        }

        #endregion


        #region find, search and paging records

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetQueryableWithFilter().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllFromQueryAsync(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public virtual async Task<IPagedRecords<T>> GetPagedRecordsAsync(IQueryable<T> query, int page, int recordsPerPage)
        {
            return await PagedRecords.Build(query, page, recordsPerPage);
        }

        public virtual async Task<IPagedRecords<T>> GetPagedRecordsMappedAsync(IQueryable<T> query, int page, int recordsPerPage)
        {
            IPagedRecords<T> pagedRecords = await GetPagedRecordsAsync(query, page, recordsPerPage);

            return MapPagedRecordsToModelDTO(pagedRecords);
        }

        public virtual async Task<T> FirstByIdAsync(long id)
        {
            IQueryable<T> query = GetQueryableWithFilter()
                .Where(x => ((IEntityModelBase)x).Id == id);

            return await FirstFromQueryAsync(query);
        }

        public virtual async Task<T> FirstFromQueryAsync(IQueryable<T> query)
        {
            T entity = await query.FirstOrDefaultAsync();

            if (entity == null)
                throw new WebApiException(System.Net.HttpStatusCode.NotFound, $"{EntityClassName} not found!");

            return entity;
        }

        #endregion


        #region modify records and save

        public async Task<T> CreateFromModelDTOAsync(IModelDTO modelDTO)
        {
            T entity = Activator.CreateInstance<T>();

            entity = await FillEntityFromModelDTO(entity, modelDTO);

            return await CreateFromEntityAsync(entity);
        }

        public virtual async Task<T> FillEntityFromModelDTO(T entity, IModelDTO modelDTO) => await Task.Run(() => entity);

        public async Task<T> CreateFromEntityAsync(T entity)
        {
            await OnBeforeCreate(entity);

            AppDbContext.Set<T>().Add(entity);

            await TrySaveChangesAsync();

            return entity;
        }

        protected virtual Task OnBeforeCreate(T entity) => Task.CompletedTask;
        
        public async Task<T> UpdateFromModelDTOAsync(long id, IModelDTO modelDTO)
        {
            T entity = await FirstByIdAsync(id);

            entity = await FillEntityFromModelDTO(entity, modelDTO);

            return await UpdateFromEntityAsync(entity);
        }

        public async Task<T> UpdateFromEntityAsync(T entity)
        {
            await OnBeforeUpdate(entity);

            AppDbContext.Entry(entity).State = EntityState.Modified;

            await TrySaveChangesAsync();

            return entity;
        }

        protected virtual Task OnBeforeUpdate(T entity) => Task.CompletedTask;

        public async Task<T> DeleteByIdAsync(long id)
        {
            T entity = await FirstByIdAsync(id);

            return await DeleteFromEntityAsync(entity);
        }

        public async Task<T> DeleteFromEntityAsync(T entity)
        {
            await OnBeforeDelete(entity);

            AppDbContext.Set<T>().Remove(entity);

            await TrySaveChangesAsync();

            return entity;
        }

        protected virtual Task OnBeforeDelete(T entity) => Task.CompletedTask;

        private async Task TrySaveChangesAsync()
        {
            try
            {
                await OnBeforeSave();
                await AppDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, $"Fails when trying to save changes to database: {e.Message}");
            }
        }

        protected virtual Task OnBeforeSave() => Task.CompletedTask;

        #endregion


        #region mapping entities to DTO and responses

        public virtual IPagedRecords<T> MapPagedRecordsToModelDTO(IPagedRecords<T> pagedRecords) => pagedRecords;

        public virtual IModelDTO MapEntityToModelDTO(T entity)
        {
            return (IModelDTO)Mapper.Map(entity, typeof(T), ShowModelDTOType);
        }

        public virtual ActionResult<WebApiResponse> MakeSuccessResponse(object data, string message = "")
        {
            return new WebApiResponse()
                .Success()
                .SetData(data)
                .SetMessage(message);
        }

        public virtual ActionResult<WebApiResponse> MapEntityAndMakeResponse(T entity, string message = "success!")
        {
            var map = MapEntityToModelDTO(entity);

            return MakeSuccessResponse(map, message);
        }

        #endregion


        #region quick access functions for controllers

        public virtual async Task<ActionResult<WebApiResponse>> GetAllPagedRecordsAndMakeResponseAsync(int page, int recordsPerPage)
        {
            return await GetPagedRecordsFromQueryAndMakeResponseAsync(GetQueryableWithFilter(), page, recordsPerPage);
        }

        public virtual async Task<ActionResult<WebApiResponse>> GetPagedRecordsFromQueryAndMakeResponseAsync(IQueryable<T> query, int page, int recordsPerPage)
        {
            IPagedRecords<T> paged = await GetPagedRecordsMappedAsync(query, page, recordsPerPage);

            paged = MapPagedRecordsToModelDTO(paged);

            return MakeSuccessResponse(paged);
        }

        public virtual async Task<ActionResult<WebApiResponse>> FirstByIdAndMakeResponseAsync(long id)
        {
            return MapEntityAndMakeResponse(await FirstByIdAsync(id));
        }

        public virtual async Task<ActionResult<WebApiResponse>> FirstFromQueryAndMakeResponseAsync(IQueryable<T> query)
        {
            return MapEntityAndMakeResponse(await FirstFromQueryAsync(query));
        }

        public virtual async Task<ActionResult<WebApiResponse>> CreateFromModelDTOAndMakeResponseAsync(IModelDTO modelDTO)
        {
            return MapEntityAndMakeResponse(await CreateFromModelDTOAsync(modelDTO));
        }

        public virtual async Task<ActionResult<WebApiResponse>> UpdateFromModelDTOAndMakeResponseAsync(long id, IModelDTO modelDTO)
        {
            return MapEntityAndMakeResponse(await UpdateFromModelDTOAsync(id, modelDTO));
        }

        public virtual async Task<ActionResult<WebApiResponse>> DeleteByIdAndMakeResponseAsync(long id)
        {
            return MapEntityAndMakeResponse(await DeleteByIdAsync(id));
        }

        #endregion
    }
}