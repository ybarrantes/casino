using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Queryable;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Casino.API.Components.UserAccounts
{
    public class OnlyTransactionsFromUserAccountQueryFilter : IQueryableFilter<AccountTransaction>
    {
        private long _userAccountId = -1;
        private long _userId = -1;

        public OnlyTransactionsFromUserAccountQueryFilter(long userId, long userAccountId)
        {
            _userAccountId = userAccountId;
            _userId = userId;
        }

        public IQueryable<AccountTransaction> SetFilter(IQueryable<AccountTransaction> query)
        {
            return query
                .Include(x => x.State)
                .Include(x => x.Type)
                .Where(x =>
                    x.UserAccount.Id.Equals(_userAccountId) &&
                    x.UserAccount.UserOwner.Id.Equals(_userId) &&
                    x.DeletedAt == null)
                .OrderByDescending(x => x.Id);
        }
    }
}
