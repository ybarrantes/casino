using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Casino.Services.DB.SQL.Queryable;

namespace Casino.Services.DB.SQL
{
    public sealed class ContextSqlCRUD<T> : IContextCRUD<T> where T : class
    {
        private DbContext _appDbContext = null;

        public ContextSqlCRUD()
        {
            CustomFilter = new QueryableDefaultFilter<T>();
            QueryablePagedRecords = new QueryableDefaultPagedRecords<T>();
        }

        #region Implemented Members

        public IQueryableFilter<T> CustomFilter { get; set; }

        public IQueryablePagedRecords<T> QueryablePagedRecords { get; set; }

        public DbContext AppDbContext
        {
            get
            {
                if (_appDbContext == null)
                    throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, "Undefined AppDbContext");

                return _appDbContext;
            }
            set => _appDbContext = value;
        }

        public async Task<T> CreateFromEntityAsync(T entity)
        {
            AppDbContext.Set<T>().Add(entity);

            await TrySaveChangesAsync();

            return entity;
        }

        public async Task<T> DeleteByEntityAsync(T entity)
        {
            AppDbContext.Set<T>().Remove(entity);

            await TrySaveChangesAsync();

            return entity;
        }

        public async Task<T> DeleteByIdAsync(long id)
        {
            T entity = await FindByIdAsync(id);

            return await DeleteByEntityAsync(entity);
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            IQueryable<T> queryable = QueryableFindAll();

            return await queryable.ToListAsync<T>();
        }

        public async Task<IPagedRecords> FindAllPagedAsync(int page, int recodsPerPage)
        {
            IQueryable<T> queryable = QueryableFindAll();

            return await QueryablePagedRecords.GetPagedRecords(queryable, page, recodsPerPage);
        }

        public async Task<T> FindByIdAsync(long id)
        {
            IQueryable<T> queryable = QueryableFindById(id);

            T entity = await queryable.FirstOrDefaultAsync();

            if (entity == null)
                throw new WebApiException(System.Net.HttpStatusCode.NotFound, $"{EntityClassName} id '{id}' not found!");

            return entity;
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> queryable = GetCustomQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<IPagedRecords> GetPagedAsync(Expression<Func<T, bool>> whereCondition = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int page = 1, int recodsPerPage = 10)
        {
            IQueryable<T> queryable = GetCustomQueryable();

            return await QueryablePagedRecords.GetPagedRecords(queryable, page, recodsPerPage);
        }

        public async Task<T> UpdateFromEntityAsync(T entity)
        {
            await FindByIdAsync(((IEntityModelBase)entity).Id);

            AppDbContext.Entry(entity).State = EntityState.Modified;

            await TrySaveChangesAsync();

            return entity;
        }

        public IQueryable<T> QueryableFindAll()
        {
            IQueryable<T> queryable = QueryableFindAllWithoutFilters();

            return CustomFilter.SetFilter(queryable);
        }

        public IQueryable<T> QueryableFindAllWithoutFilters()
        {
            return AppDbContext.Set<T>().AsQueryable();
        }

        public IQueryable<T> QueryableFindById(long id)
        {
            IQueryable<T> queryable = AppDbContext.Set<T>().AsQueryable();

            queryable = QueryableFindById(queryable, id);

            return CustomFilter.SetFilter(queryable);
        }

        public IQueryable<T> QueryableFindByIdWithoutFilters(long id)
        {
            IQueryable<T> queryable = AppDbContext.Set<T>().AsQueryable();

            return QueryableFindById(queryable, id);
        }


        #endregion

        private string EntityClassName => typeof(T).Name;

        private IQueryable<T> QueryableFindById(IQueryable<T> queryable, long id)
        {
            return queryable.Where<T>(r => ((IEntityModelBase)r).Id.Equals(id));
        }

        private IQueryable<T> GetCustomQueryable(Expression<Func<T, bool>> whereCondition = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (whereCondition != null)
            {
                query = query.Where(whereCondition);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        private async Task TrySaveChangesAsync()
        {
            try
            {
                await AppDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, $"Fails when trying to save changes to database: {e.Message}");
            }
        }
    }
}
