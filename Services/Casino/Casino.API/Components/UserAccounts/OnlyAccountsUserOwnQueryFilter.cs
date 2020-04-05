using Casino.Data.Models.Default;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Queryable;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Casino.API.Components.UserAccounts
{
    public class OnlyAccountsUserOwnQueryFilter : IQueryableFilter<UserAccount>
    {
        private long _userId = -1;

        public OnlyAccountsUserOwnQueryFilter(long userId)
        {
            _userId = userId;
        }

        public IQueryable<UserAccount> SetFilter(IQueryable<UserAccount> query)
        {
            return query
                .Include(x => x.State)
                .Include(x => x.Type)
                .Where(x =>
                    x.UserOwner.Id.Equals(_userId) &&
                    x.DeletedAt == null);
        }
    }
}
