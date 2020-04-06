using Casino.Data.Models.DTO.AccountTransanctions;
using Casino.Data.Models.Entities;
using System.Threading.Tasks;

namespace Casino.API.Components.AccountTransactions
{
    public interface IAccountTransactionComponent
    {
        Task<AccountTransaction> SetAccountTransactionAsync(UserAccount userAccount, AccountTransactionCreateDTO modelDTO);
    }
}
