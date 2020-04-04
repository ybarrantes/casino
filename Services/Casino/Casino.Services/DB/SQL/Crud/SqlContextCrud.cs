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
        public IQueryableFilter<T> QueryableFilter { get; set; }
        protected Type ShowModelDTOType { get; set; }

        public SqlContextCrud(IMapper mapper, IPagedRecords<T> pagedRecords)
        {
            _mapper = mapper;
            _pagedRecords = pagedRecords;

            QueryableFilter = new QueryableDefaultFilter<T>();
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return QueryableFilter.SetFilter(GetQueryableWithoutFilter());
        }

        public virtual IQueryable<T> GetQueryableWithoutFilter()
        {
            return AppDbContext.Set<T>();
        }


        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await GetQueryable().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllFromQuery(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public virtual async Task<IPagedRecords<T>> GetPagedRecords(IQueryable<T> query, int page, int recordsPerPage)
        {
            return await PagedRecords.Build(query, page, recordsPerPage);
        }

        public virtual async Task<IPagedRecords<T>> GetPagedRecordsMapped(IQueryable<T> query, int page, int recordsPerPage)
        {
            IPagedRecords<T> pagedRecords = await GetPagedRecords(query, page, recordsPerPage);

            return MapPagedRecordsToModelDTO(pagedRecords);
        }

        public virtual async Task<T> FirstById(long id)
        {
            IQueryable<T> query = GetQueryable()
                .Where(x => ((IEntityModelBase)x).Id == id);

            return await FirstFromQuery(query);
        }

        public virtual async Task<T> FirstFromQuery(IQueryable<T> query)
        {
            T entity = await query.FirstOrDefaultAsync();

            if (entity == null)
                throw new WebApiException(System.Net.HttpStatusCode.NotFound, $"{EntityClassName} not found!");

            return entity;
        }



        public async Task<T> CreateFromModelDTOAsync(IModelDTO modelDTO)
        {
            T entity = Activator.CreateInstance<T>();

            entity = await FillEntityFromModelDTO(entity, modelDTO);

            return await CreateFromEntityAsync(entity);
        }

        public async Task<T> CreateFromEntityAsync(T entity)
        {
            await OnBeforeCreate(entity);

            AppDbContext.Set<T>().Add(entity);

            await TrySaveChangesAsync();

            return entity;
        }

        protected virtual Task OnBeforeCreate(T entity) => Task.CompletedTask;


        public async Task<T> UpdateFromEntityAsync(T entity)
        {
            await OnBeforeUpdate(entity);

            AppDbContext.Entry(entity).State = EntityState.Modified;

            await TrySaveChangesAsync();

            return entity;
        }

        public async Task<T> UpdateFromModelDTOAsync(long id, IModelDTO modelDTO)
        {
            T entity = await FirstById(id);

            entity = await FillEntityFromModelDTO(entity, modelDTO);

            return await CreateFromEntityAsync(entity);
        }

        protected virtual Task OnBeforeUpdate(T entity) => Task.CompletedTask;


        public async Task<T> DeleteByIdAsync(long id)
        {
            T entity = await FirstById(id);

            return await DeleteFromEntityAsync(entity);
        }

        public async Task<T> DeleteFromEntityAsync(T entity)
        {
            await OnBeforeDelete(entity);

            AppDbContext.Set<T>().Remove(entity);

            await TrySaveChangesAsync();

            return entity;
        }

        protected virtual Task OnBeforeDelete(T entity)
        {
            return Task.CompletedTask;
        }


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

        public virtual ActionResult<WebApiResponse> MapEntityAndResponse(T entity, string message = "success!")
        {
            var map = MapEntityToModelDTO(entity);

            return MakeSuccessResponse(map, message);
        }

        public virtual async Task<T> FillEntityFromModelDTO(T entity, IModelDTO modelDTO) => await Task.Run(() => entity);



        public virtual async Task<ActionResult<WebApiResponse>> GetAllPagedRecordsAndResponseAsync(int page, int recordsPerPage)
        {
            return await GetPagedRecordsAndResponseAsync(GetQueryable(), page, recordsPerPage);
        }

        public virtual async Task<ActionResult<WebApiResponse>> GetPagedRecordsAndResponseAsync(IQueryable<T> query, int page, int recordsPerPage)
        {
            IPagedRecords<T> paged = await GetPagedRecordsMapped(query, page, recordsPerPage);

            paged = MapPagedRecordsToModelDTO(paged);

            return MakeSuccessResponse(paged);
        }

        public virtual async Task<ActionResult<WebApiResponse>> FirstByIdAndResponseAsync(long id)
        {
            return MapEntityAndResponse(await FirstById(id));
        }

        public virtual async Task<ActionResult<WebApiResponse>> FirstFromQueryAndResponseAsync(IQueryable<T> query)
        {
            return MapEntityAndResponse(await FirstFromQuery(query));
        }

        public virtual async Task<ActionResult<WebApiResponse>> CreateFromModelDTOAndResponseAsync(IModelDTO modelDTO)
        {
            return MapEntityAndResponse(await CreateFromModelDTOAsync(modelDTO));
        }

        public virtual async Task<ActionResult<WebApiResponse>> UpdateFromModelDTOAndResponseAsync(long id, IModelDTO modelDTO)
        {
            return MapEntityAndResponse(await UpdateFromModelDTOAsync(id, modelDTO));
        }

        public virtual async Task<ActionResult<WebApiResponse>> DeleteByIdAndResponseAsync(long id)
        {
            return MapEntityAndResponse(await DeleteByIdAsync(id));
        }
    }
}