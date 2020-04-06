using AutoMapper;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.DB.SQL.Queryable;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Services.DB.SQL.Crud
{
    public interface ISqlContextCrud<T> : IDbContext where T : class
    {
        #region Core

        IMapper Mapper { get; }

        Type ShowModelDTOType { get; set; }

        IQueryableFilter<T> QueryFilter { get; set; }

        IQueryable<T> GetQueryableWithFilter();
        IQueryable<T> GetQueryableWithoutFilter();

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllFromQueryAsync(IQueryable<T> query);

        Task<IPagedRecords<T>> GetPagedRecordsAsync(IQueryable<T> query, int page, int recordsPerPage);
        Task<IPagedRecords<T>> GetPagedRecordsMappedAsync(IQueryable<T> query, int page, int recordsPerPage);

        Task<T> FirstByIdAsync(long id);
        Task<T> FirstFromQueryAsync(IQueryable<T> query);

        Task<T> CreateFromModelDTOAsync(IModelDTO modelDTO);
        Task<T> CreateFromEntityAsync(T entity);

        Task<T> UpdateFromModelDTOAsync(long id, IModelDTO modelDTO);
        Task<T> UpdateFromEntityAsync(T entity);

        Task<T> DeleteByIdAsync(long id);
        Task<T> DeleteFromEntityAsync(T entity);

        IPagedRecords<T> MapPagedRecordsToModelDTO(IPagedRecords<T> pagedRecords);
        IModelDTO MapEntityToModelDTO(T entity);

        ActionResult<WebApiResponse> MakeSuccessResponse(object data, string message = "");
        ActionResult<WebApiResponse> MapEntityAndMakeResponse(T entity, string message = "success!");

        #endregion

        #region quick access functions for controllers

        Task<ActionResult<WebApiResponse>> GetAllPagedRecordsAndMakeResponseAsync(int page, int recordsPerPage);
        Task<ActionResult<WebApiResponse>> GetPagedRecordsFromQueryAndMakeResponseAsync(IQueryable<T> query, int page, int recordsPerPage);
        
        Task<ActionResult<WebApiResponse>> FirstByIdAndMakeResponseAsync(long id);
        Task<ActionResult<WebApiResponse>> FirstFromQueryAndMakeResponseAsync(IQueryable<T> query);
        Task<ActionResult<WebApiResponse>> CreateFromModelDTOAndMakeResponseAsync(IModelDTO modelDTO);
        Task<ActionResult<WebApiResponse>> UpdateFromModelDTOAndMakeResponseAsync(long id, IModelDTO modelDTO);
        Task<ActionResult<WebApiResponse>> DeleteByIdAndMakeResponseAsync(long id);

        #endregion
    }
}
