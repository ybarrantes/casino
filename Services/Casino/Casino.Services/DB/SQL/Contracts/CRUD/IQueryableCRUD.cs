using System.Linq;

namespace Casino.Services.DB.SQL.Contracts.CRUD
{
    public interface IQueryableCRUD<T> where T : class
    {
        IQueryable<T> QueryableFindAll();
        IQueryable<T> QueryableFindAllWithoutFilters();

        IQueryable<T> QueryableFindById(long id);
        IQueryable<T> QueryableFindByIdWithoutFilters(long id);

        IQueryable<T> QueryableAddQueryFilters(IQueryable<T> query);
    }
}
