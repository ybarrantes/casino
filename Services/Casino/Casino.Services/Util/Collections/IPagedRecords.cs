using System.Collections;
using System.Threading.Tasks;

namespace Casino.Services.Util.Collections
{
    public interface IPagedRecords
    {
        int RecordsPerPage { get; set; }
        int TotalRecords { get; }
        int Page { get; set; }
        int TotalPages { get; }
        IEnumerable Result { get; set; }

        Task Build();
    }
}
