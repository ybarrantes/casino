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
using Casino.Data.Models.Default;

namespace Casino.API.Components.UserAccounts
{
    public class UserAccountsCrudComponent : SqlContextCrud<UserAccount>, IUserAccountComponent
    {
        private readonly IIdentityApp<User> _identityApp;
        private readonly ISqlContextCrud<UserAccountBalance> _sqlUserAccountCrudComponent;

        public UserAccountsCrudComponent(
            IMapper mapper,
            IPagedRecords<UserAccount> pagedRecords,
            IIdentityApp<User> identityApp,
            ISqlContextCrud<UserAccountBalance> sqlContextCrud)
            : base(mapper, pagedRecords)
        {
            _identityApp = identityApp;
            _sqlUserAccountCrudComponent = sqlContextCrud;

            ShowModelDTOType = typeof(UserAccountShowDTO);
            _sqlUserAccountCrudComponent.ShowModelDTOType = typeof(UserAccountShowDTO);
        }

        public override IPagedRecords<UserAccount> MapPagedRecordsToModelDTO(IPagedRecords<UserAccount> pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<UserAccountShowDTO>>(pagedRecords.Result);

            return pagedRecords;
        }

        protected override void OnDbContextChange(ApplicationDbContextBase dbContext)
        {
            _sqlUserAccountCrudComponent.AppDbContext = dbContext;
        }

        /// <summary>
        /// Apply welcome bonus on register a UserAccountEntity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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

            IQueryable<UserAccountBalance> query = _sqlUserAccountCrudComponent.GetQueryableWithFilter();

            IPagedRecords<UserAccountBalance> pagedRecords = await _sqlUserAccountCrudComponent.GetPagedRecordsAsync(query, page, 20);

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

        private async Task AbortIfAuthUserIsNotOwnerAccount(long userOwnerAccountId)
        {
            User userSigned = await _identityApp.GetUser(AppDbContext);

            if (userOwnerAccountId != userSigned.Id)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "access denied");
        }

        private void SetFilterCrudComponent(long userId)
        {
            _sqlUserAccountCrudComponent.QueryFilter = new UserAccountsBalanceQueryFilter(AppDbContext, userId);
        }

        public async Task<ActionResult<WebApiResponse>> GetOneUserAccountsAsync(long userId, long accountId)
        {
            await AbortOnUserNotExistsAsync(userId);

            SetFilterCrudComponent(userId);

            IQueryable<UserAccountBalance> query = _sqlUserAccountCrudComponent.GetQueryableWithFilter()
                .Where(x => x.Id == accountId);

            return await _sqlUserAccountCrudComponent.FirstFromQueryAndMakeResponseAsync(query);
        }



        public async Task<ActionResult<WebApiResponse>> SetAccountTransactionAsync(long userId, long accountId, AccountTransactionCreateDTO modelDTO)
        {
            await AbortOnUserNotExistsAsync(userId);

            await CheckIfTransactionIsAvilable(userId, modelDTO);

            QueryFilter = new OnlyAccountsUserOwnQueryFilter(userId);

            UserAccount userAccount = await FirstByIdAsync(accountId);

            AccountTransaction accountTransaction = await SaveAccountTransaction(userAccount, (AccountTransactionTypes)modelDTO.Type, modelDTO.Amount);

            return await GetOneUserAccountsAsync(userId, accountId);
        }

        private async Task CheckIfTransactionIsAvilable(long userId, AccountTransactionCreateDTO modelDTO)
        {
            bool failTransaction = false;

            if (modelDTO.Amount <= 0)
                failTransaction = true;
            else if (modelDTO.Type == (long)AccountTransactionTypes.Deposit || modelDTO.Type == (long)AccountTransactionTypes.Withdrawal)
                await AbortIfAuthUserIsNotOwnerAccount(userId);
            else if (modelDTO.Type == (long)AccountTransactionTypes.Bonus)
                await CheckIfAuthUserIsSuperAdmin();
            else
                failTransaction = true;

            if(failTransaction)
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, "invalid operation");
        }

        private Task CheckIfAuthUserIsSuperAdmin()
        {
            // TODO: validate when auth user not is super admin for throw exception
            return Task.CompletedTask;
        }

        private async Task<AccountTransaction> SaveAccountTransaction(UserAccount userAccount, AccountTransactionTypes transactionType, decimal amount)
        {
            // when transaction is withdrawal type then amount becomes negative
            if (transactionType == AccountTransactionTypes.Withdrawal)
                amount *= -1;

            User authUser = await _identityApp.GetUser(AppDbContext);

            AccountTransaction accountTransaction = new AccountTransaction
            {
                UserAccount = userAccount,
                Amount = amount,
                UserRegister = authUser,
                State = await AppDbContext
                    .FindGenericElementByIdAsync<AccountTransactionState>((long)AccountTransactionStates.Approved),
                Type = await AppDbContext
                    .FindGenericElementByIdAsync<AccountTransactionType>((long)transactionType)
            };

            AppDbContext.Set<AccountTransaction>().Add(accountTransaction);

            await AppDbContext.SaveChangesAsync();

            return accountTransaction;
        }
    }
}
