using Casino.API.Components.UserAccounts;
using Casino.Data.Context;
using Casino.Data.Models.DTO.AccountTransanctions;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Casino.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users/{userId}/accounts")]
    public class UserAccountsController : ControllerBase
    {
        private readonly ISqlContextCrud<UserAccount> _userAccountCrudComponent;

        public UserAccountsController(
            ApplicationDbContext dbContext,
            ISqlContextCrud<UserAccount> userAccountCrudComponent)
        {
            _userAccountCrudComponent = userAccountCrudComponent;
            _userAccountCrudComponent.AppDbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetAllAccountOfUser(long userId, int page = 1)
        {            
            return await ((IUserAccountComponent)_userAccountCrudComponent).GetAllUserAccountsPagedRecordsAsync(userId, page);
        }

        [HttpGet("{accountId}")]
        public async Task<ActionResult<WebApiResponse>> GetOneUserAccount(long userId, long accountId)
        {
            return await ((IUserAccountComponent)_userAccountCrudComponent).GetOneUserAccountsAsync(userId, accountId);
        }

        [HttpGet("{accountId}/transactions")]
        public async Task<ActionResult<WebApiResponse>> GetAllAccountTransactions(long userId, long accountId, int page = 1)
        {
            return await ((IUserAccountComponent)_userAccountCrudComponent)
                .GetAllAccountTransactionsPagedRecordsAsync(userId, accountId, page);
        }

        [HttpPost("{accountId}/transactions")]
        public async Task<ActionResult<WebApiResponse>> SetAccountTransaction(
            long userId, long accountId, [FromBody] AccountTransactionCreateDTO data)
        {
            return await ((IUserAccountComponent)_userAccountCrudComponent)
                .SetAccountTransactionAsync(userId, accountId, data);
        }
    }
}
