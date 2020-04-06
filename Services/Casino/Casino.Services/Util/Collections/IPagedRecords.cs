using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Services.Util.Collections
{
    public interface IPagedRecords<T> where T : class
    {
        int RecordsPerPage { get; }
        int TotalRecords { get; }
        int Page { get; }
        int TotalPages { get; }
        IEnumerable Result { get; set; }

        Task<IPagedRecords<T>> Build(IQueryable<T> entityQueryBuilder, int page, int recodsPerPage);
    }
}
