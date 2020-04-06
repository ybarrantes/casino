using Casino.Data.Models.Enums;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Context;
using System.Threading.Tasks;

namespace Casino.API.BusisnessLogic
{
    public class ApplyWelcomeBonus : IApplyBonus
    {
        private ApplicationDbContextBase _dbContext = null;

        public async Task<AccountTransaction> ApplyBonus(
            ApplicationDbContextBase dbContext,
            UserAccount userAccount)
        {
            _dbContext = dbContext;

            return await ApplyWelcomeBonusTransaction(userAccount);
        }

        private async Task<AccountTransaction> ApplyWelcomeBonusTransaction(UserAccount userAccount)
        {
            AccountTransaction accountTransaction = null;

            decimal amount = GetWelcomeBonusAmmountByUserAccountType(userAccount.Type);

            if (amount > 0M)
            {
                accountTransaction = await FillAccountTransaction(userAccount);

                accountTransaction.Amount = amount;

                _dbContext.Set<AccountTransaction>().Add(accountTransaction);

                await _dbContext.SaveChangesAsync();
            }

            return accountTransaction;
        }

        private decimal GetWelcomeBonusAmmountByUserAccountType(UserAccountType accountType)
        {
            if (accountType.Id == (long)UserAccountTypes.Real)
                return 2000.0M;
            else if (accountType.Id == (long)UserAccountTypes.Free)
                return 30.0M;

            return 0M;
        }

        private async Task<AccountTransaction> FillAccountTransaction(UserAccount userAccount)
        {
            return new AccountTransaction
            {
                UserAccount = userAccount,
                // TODO: change user owner by user system default (1)
                UserRegister = userAccount.UserOwner,
                State = await _dbContext.FindGenericElementByIdAsync<AccountTransactionState>(
                        (long)AccountTransactionStates.Approved),
                Type = await _dbContext.FindGenericElementByIdAsync<AccountTransactionType>(
                        (long)AccountTransactionTypes.Bonus)
            };
        }   
    }
}
