using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.Util.Collections;

namespace Casino.Services.DB.SQL.Contracts.CRUD
{
    public interface IContextCRUD<T> : IQueryableCRUD<T>, IPagedCRUD<T> where T : class
    {
        DbContext AppDbContext { get; set; }

        Task<IEnumerable<T>> FindAllAsync();
        
        Task<IPagedRecords> FindAllPagedAsync(int page, int recodsPerPage);

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        Task<IPagedRecords> GetPagedAsync(Expression<Func<T, bool>> whereCondition = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int page = 1, int recodsPerPage = 10);


        Task<T> FindByIdAsync(long id);
        Task<T> DeleteByIdAsync(long id);


        Task<T> DeleteByEntityAsync(T entity);
        Task<T> CreateFromEntityAsync(T entity);
        Task<T> UpdateFromEntityAsync(T entity);
    }
}
