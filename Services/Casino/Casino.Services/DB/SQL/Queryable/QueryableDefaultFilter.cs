using Casino.Services.DB.SQL.Contracts.Model;
using System.Collections.Generic;
using System.Linq;

namespace Casino.Services.DB.SQL.Queryable
{
    public sealed class QueryableDefaultFilter<T> : IQueryableFilter<T> where T : class
    {
        public IQueryable<T> SetFilter(IQueryable<T> query) => query;
    }
}
