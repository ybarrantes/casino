using Casino.Data.Models.Default;
using Casino.Data.Models.Entities;
using Casino.Data.Models.Views;
using Casino.Services.DB.SQL.Context;
using Casino.Services.DB.SQL.Queryable;
using System.Linq;

namespace Casino.API.Components.UserAccounts
{
    public class UserAccountsBalanceQueryFilter : IQueryableFilter<UserAccountBalance>
    {
        private long _userOwnerId = -1;
        private readonly ApplicationDbContextBase _dbContext = null;

        public UserAccountsBalanceQueryFilter(ApplicationDbContextBase dbContext)
        {
            _dbContext = dbContext;
        }

        public UserAccountsBalanceQueryFilter(ApplicationDbContextBase dbContext, long userOwnerId) : this(dbContext)
        {
            _userOwnerId = userOwnerId;
        }

        public IQueryable<UserAccountBalance> SetFilter(IQueryable<UserAccountBalance> query)
        {
            query = (
                from a in _dbContext.Set<UserAccount>()
                join va in _dbContext.Set<UserAccountBalance>() on a.Id equals va.Id
                where
                    a.UserOwner.Id == _userOwnerId &&
                    a.State.Id == (long)UserAccountStates.Active &&
                    a.DeletedAt == null
                select new UserAccountBalance
                {
                    Id = a.Id,
                    CreatedAt = a.CreatedAt,
                    State = a.State,
                    Type = a.Type,
                    UserOwner = a.UserOwner,
                    TotalBalance = va.TotalBalance,
                    AvailableBalance = va.AvailableBalance,
                    BetBalance = va.BetBalance
                }
                ).AsQueryable();

            return query;
        }
    }
}
