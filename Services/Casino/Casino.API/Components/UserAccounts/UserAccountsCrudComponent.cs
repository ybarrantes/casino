using AutoMapper;
using Casino.Data.Models.DTO.UserAccounts;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Casino.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Casino.Services.WebApi;
using System.Linq;
using Casino.Data.Models.Views;
using Casino.API.BusisnessLogic;
using Casino.Services.DB.SQL.Context;
using Casino.Data.Models.DTO.AccountTransanctions;
using Casino.API.Components.AccountTransactions;

namespace Casino.API.Components.UserAccounts
{
    public class UserAccountsCrudComponent : SqlContextCrud<UserAccount>, IUserAccountComponent
    {
        private readonly IIdentityApp<User> _identityApp;
        private readonly ISqlContextCrud<UserAccountBalance> _sqlUserAccountBalanceCrudComponent;
        private readonly ISqlContextCrud<AccountTransaction> _sqlAccountTransactionContextCrud;

        public UserAccountsCrudComponent(
            IMapper mapper,
            IPagedRecords<UserAccount> pagedRecords,
            IIdentityApp<User> identityApp,
            ISqlContextCrud<UserAccountBalance> sqlUserAccountBalanceContextCrud,
            ISqlContextCrud<AccountTransaction> sqlAccountTransactionContextCrud)
            : base(mapper, pagedRecords)
        {
            _identityApp = identityApp;
            _sqlUserAccountBalanceCrudComponent = sqlUserAccountBalanceContextCrud;
            _sqlAccountTransactionContextCrud = sqlAccountTransactionContextCrud;

            ShowModelDTOType = typeof(UserAccountShowDTO);
            _sqlUserAccountBalanceCrudComponent.ShowModelDTOType = typeof(UserAccountShowDTO);
        }

        public override IPagedRecords<UserAccount> MapPagedRecordsToModelDTO(IPagedRecords<UserAccount> pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<UserAccountShowDTO>>(pagedRecords.Result);

            return pagedRecords;
        }

        protected override void OnDbContextChange(ApplicationDbContextBase dbContext)
        {
            _sqlUserAccountBalanceCrudComponent.AppDbContext = dbContext;
            _sqlAccountTransactionContextCrud.AppDbContext = dbContext;
        }

        /// Apply welcome bonus on register a UserAccountEntity
        protected override async Task OnAfterCreate(UserAccount entity)
        {
            // TODO: use dependency injection
            IApplyBonus applyBonus = new ApplyWelcomeBonus();
                
            await applyBonus.ApplyBonus(AppDbContext, entity);
        }
        

        public async Task<ActionResult<WebApiResponse>> GetAllUserAccountsPagedRecordsAsync(long userId, int page)
        {
            await AbortOnUserNotExistsAsync(userId);

            SetFilterCrudComponent(userId);

            IQueryable<UserAccountBalance> query = _sqlUserAccountBalanceCrudComponent.GetQueryableWithFilter();

            IPagedRecords<UserAccountBalance> pagedRecords = await _sqlUserAccountBalanceCrudComponent.GetPagedRecordsAsync(query, page, 20);

            pagedRecords.Result = Mapper.Map<List<UserAccountShowDTO>>(pagedRecords.Result);

            return MakeSuccessResponse(pagedRecords);
        }
        
        private async Task<User> AbortOnUserNotExistsAsync(long userId)
        {
            User user = await AppDbContext.Set<User>()
                .FirstOrDefaultAsync(
                    x => x.Id == userId && x.DeletedAt == null);

            if (user == null)
                throw new WebApiException(System.Net.HttpStatusCode.NotFound, "user not found!");

            return user;
        }

        private void SetFilterCrudComponent(long userId)
        {
            _sqlUserAccountBalanceCrudComponent.QueryFilter = new UserAccountsBalanceQueryFilter(AppDbContext, userId);
        }
                      
        public async Task<ActionResult<WebApiResponse>> GetOneUserAccountsAsync(long userId, long accountId)
        {
            await AbortOnUserNotExistsAsync(userId);

            SetFilterCrudComponent(userId);

            IQueryable<UserAccountBalance> query = _sqlUserAccountBalanceCrudComponent.GetQueryableWithFilter()
                .Where(x => x.Id == accountId);

            return await _sqlUserAccountBalanceCrudComponent.FirstFromQueryAndMakeResponseAsync(query);
        }


        public async Task<ActionResult<WebApiResponse>> SetAccountTransactionAsync(long userId, long accountId, AccountTransactionCreateDTO modelDTO)
        {
            await AbortOnUserNotExistsAsync(userId);

            QueryFilter = new OnlyOwnerUserAccountsQueryFilter(userId);

            UserAccount userAccount = await FirstByIdAsync(accountId);

            await ((IAccountTransactionComponent)_sqlAccountTransactionContextCrud)
                .SetAccountTransactionAsync(userAccount, modelDTO);

            return await GetOneUserAccountsAsync(userId, accountId);
        }

        public async Task<ActionResult<WebApiResponse>> GetAllAccountTransactionsPagedRecordsAsync(long userId, long accountId, int page)
        {
            await AbortOnUserNotExistsAsync(userId);

            QueryFilter = new OnlyOwnerUserAccountsQueryFilter(userId);

            await FirstByIdAsync(accountId);

            _sqlAccountTransactionContextCrud.QueryFilter = new OnlyTransactionsFromUserAccountQueryFilter(userId, accountId);

            return await _sqlAccountTransactionContextCrud.GetAllPagedRecordsAndMakeResponseAsync(page, 20);
        }
    }
}
