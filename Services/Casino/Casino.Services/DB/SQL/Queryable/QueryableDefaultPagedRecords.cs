using Casino.Services.Util.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Services.DB.SQL.Queryable
{
    public sealed class QueryableDefaultPagedRecords<T> : IQueryablePagedRecords<T> where T : class
    {
        public async Task<IPagedRecords> GetPagedRecords(IQueryable<T> query, int page, int recordsPerPage)
        {
            IPagedRecords pagedRecords = new PagedRecords<T>(query, page, recordsPerPage);

            await pagedRecords.Build();

            return pagedRecords;
        }
    }
}
