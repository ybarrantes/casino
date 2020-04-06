using AutoMapper;
using Casino.API.Services;
using Casino.Data.Models.DTO.AccountTransanctions;
using Casino.Data.Models.Entities;
using Casino.Data.Models.Enums;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.API.Components.AccountTransactions
{
    public class AccountTransactionsCrudComponent : SqlContextCrud<AccountTransaction>, IAccountTransactionComponent
    {
        private readonly IIdentityApp<User> _identityApp = null;

        public AccountTransactionsCrudComponent(
            IMapper mapper,
            IPagedRecords<AccountTransaction> pagedRecords,
            IIdentityApp<User> identityApp)
            : base(mapper, pagedRecords)
        {
            _identityApp = identityApp;

            ShowModelDTOType = typeof(AccountTransactionShowDTO);
        }

        public override IPagedRecords<AccountTransaction> MapPagedRecordsToModelDTO(IPagedRecords<AccountTransaction> pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<AccountTransactionShowDTO>>(pagedRecords.Result);

            return pagedRecords;
        }

        public async Task<AccountTransaction> SetAccountTransactionAsync(UserAccount userAccount, AccountTransactionCreateDTO modelDTO)
        {
            await CheckIfTransactionIsAvilable(userAccount.UserOwner.Id, modelDTO);

            return await SaveAccountTransaction(userAccount, (AccountTransactionTypes)modelDTO.Type, modelDTO.Amount);
        }

        private async Task CheckIfTransactionIsAvilable(long userId, AccountTransactionCreateDTO modelDTO)
        {
            bool failTransaction = false;

            if (modelDTO.Amount <= 0)
                failTransaction = true;
            else if (modelDTO.Type == (long)AccountTransactionTypes.Deposit || modelDTO.Type == (long)AccountTransactionTypes.Withdrawal)
                await AbortIfAuthUserIsNotOwnerAccountAsync(userId);
            else if (modelDTO.Type == (long)AccountTransactionTypes.Bonus)
                await CheckIfAuthUserIsSuperAdmin();
            else
                failTransaction = true;

            if (failTransaction)
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, "invalid operation");
        }

        private async Task AbortIfAuthUserIsNotOwnerAccountAsync(long userOwnerAccountId)
        {
            User userSigned = await _identityApp.GetUser(AppDbContext);

            if (userOwnerAccountId != userSigned.Id)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "access denied");
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

            AccountTransaction accountTransaction = new AccountTransaction
            {
                UserAccount = userAccount,
                Amount = amount,
                UserRegister = await _identityApp.GetUser(AppDbContext),
                State = await AppDbContext
                    .FindGenericElementByIdAsync<AccountTransactionState>((long)AccountTransactionStates.Approved),
                Type = await AppDbContext
                    .FindGenericElementByIdAsync<AccountTransactionType>((long)transactionType)
            };

            return await CreateFromEntityAsync(accountTransaction);
        }
    }
}
