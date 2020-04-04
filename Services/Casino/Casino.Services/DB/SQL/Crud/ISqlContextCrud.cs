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
    public interface ISqlContextCrud<T> : IDbContext where T : class
    {
        IMapper Mapper { get; }

        IQueryableFilter<T> QueryableFilter { get; set; }

        IQueryable<T> GetQueryable();
        IQueryable<T> GetQueryableWithoutFilter();

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllFromQuery(IQueryable<T> query);

        Task<IPagedRecords<T>> GetPagedRecords(IQueryable<T> query, int page, int recordsPerPage);
        Task<IPagedRecords<T>> GetPagedRecordsMapped(IQueryable<T> query, int page, int recordsPerPage);

        Task<T> FirstById(long id);
        Task<T> FirstFromQuery(IQueryable<T> query);

        Task<T> CreateFromModelDTOAsync(IModelDTO modelDTO);
        Task<T> CreateFromEntityAsync(T entity);

        Task<T> UpdateFromModelDTOAsync(long id, IModelDTO modelDTO);
        Task<T> UpdateFromEntityAsync(T entity);

        Task<T> DeleteByIdAsync(long id);
        Task<T> DeleteFromEntityAsync(T entity);

        IPagedRecords<T> MapPagedRecordsToModelDTO(IPagedRecords<T> pagedRecords);
        IModelDTO MapEntityToModelDTO(T entity);

        ActionResult<WebApiResponse> MakeSuccessResponse(object data, string message = "");
        ActionResult<WebApiResponse> MapEntityAndResponse(T entity, string message = "success!");

        Task<ActionResult<WebApiResponse>> GetAllPagedRecordsAndResponseAsync(int page, int recordsPerPage);
        Task<ActionResult<WebApiResponse>> GetPagedRecordsAndResponseAsync(IQueryable<T> query, int page, int recordsPerPage);
        
        Task<ActionResult<WebApiResponse>> FirstByIdAndResponseAsync(long id);
        Task<ActionResult<WebApiResponse>> FirstFromQueryAndResponseAsync(IQueryable<T> query);
        Task<ActionResult<WebApiResponse>> CreateFromModelDTOAndResponseAsync(IModelDTO modelDTO);
        Task<ActionResult<WebApiResponse>> UpdateFromModelDTOAndResponseAsync(long id, IModelDTO modelDTO);
        Task<ActionResult<WebApiResponse>> DeleteByIdAndResponseAsync(long id);
    }
}
