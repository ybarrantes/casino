using Casino.Services.DB.SQL.Contracts.Model;
using System.Collections.Generic;
using System.Linq;

namespace Casino.Services.DB.SQL.Queryable
{
    public interface IQueryableFilter<T> where T : class
    {
        IQueryable<T> SetFilter(IQueryable<T> query);
    }
}
