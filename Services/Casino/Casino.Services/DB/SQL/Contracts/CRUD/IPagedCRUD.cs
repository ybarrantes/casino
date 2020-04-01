using System.Linq;
using System.Threading.Tasks;
using Casino.Services.Util.Collections;

namespace Casino.Services.DB.SQL.Contracts.CRUD
{
    public interface IPagedCRUD<T> where T : class
    {
        Task<IPagedRecords> GetPagedRecords(IQueryable<T> query, int page, int recordsPerPage);
    }
}
