using System.Linq;

namespace Casino.Services.DB.SQL.Queryable
{
    public interface IQueryableCRUD<T> where T : class
    {
        IQueryableFilter<T> CustomFilter { get; set; }

        IQueryable<T> QueryableFindAll();
        IQueryable<T> QueryableFindAllWithoutFilters();

        IQueryable<T> QueryableFindById(long id);
        IQueryable<T> QueryableFindByIdWithoutFilters(long id);
    }
}
