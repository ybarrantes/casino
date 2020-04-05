using Casino.Data.Models.DTO.UserAccounts;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Casino.API.Components.UserAccounts
{
    public interface IUserAccountComponent
    {
        Task<ActionResult<WebApiResponse>> GetAllUserAccountsPagedRecordsAsync(long userId, int page);
        Task<ActionResult<WebApiResponse>> GetOneUserAccountsAsync(long userId, long accountId);

        Task<ActionResult<WebApiResponse>> SetAccountTransactionAsync(long userId, long accountId, AccountTransactionCreateDTO modelDTO);
    }
}
