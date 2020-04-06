using AutoMapper;
using Casino.API.Services;
using Casino.Data.Models.DTO.Bets;
using Casino.Data.Models.Entities;
using Casino.Data.Models.Enums;
using Casino.Data.Models.Views;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Bets
{
    public class BetsCrudComponent : SqlContextCrud<Bet>, IBetComponent
    {
        private readonly IIdentityApp<User> _identityApp;

        public BetsCrudComponent(
            IMapper mapper,
            IPagedRecords<Bet> pagedRecords,
            IIdentityApp<User> identityApp)
            : base(mapper, pagedRecords)
        {
            _identityApp = identityApp;

            ShowModelDTOType = typeof(BetShowDTO);
        }


        public async Task<ActionResult<WebApiResponse>> CreateBet(long roundId, BetCreateDTO betCreateDTO)
        {
            UserAccount userAccount = await GetUserAccountOrAbortIfAuthUserIsNotAccountOwner(betCreateDTO.AccountId);

            await AbortIfAccountHasInsufficientFundsOrInputAmountIsZero(userAccount, betCreateDTO.Amount);

            Round round = await GetRoundOrAbortIfRoundStatusClosed(roundId);

            RouletteRule rouletteRule = await GetRouletteRuleOrAbortIfNotFound(betCreateDTO, round);            

            await AppDbContext.BeginTransactionAsync();

            AccountTransaction accountTransaction = await GetAndSaveAccountTransaction(userAccount, betCreateDTO.Amount);            

            Bet bet = new Bet
            {
                AccountTransaction = accountTransaction,
                RouletteRule = rouletteRule,
                PaymentRatio = rouletteRule.Pay,
                Round = round,
                UserRegister = accountTransaction.UserRegister,
                State = await AppDbContext.FindGenericElementByIdAsync<BetState>((long)BetStates.Active)
            };

            await CreateFromEntityAsync(bet);

            await SaveBetNumbersList(bet, betCreateDTO.Numbers);

            await AppDbContext.CommitTransactionAsync();

            return MapEntityAndMakeResponse(bet);
        }

        private async Task<Round> GetRoundOrAbortIfRoundStatusClosed(long roundId)
        {
            Round round = await AppDbContext.Set<Round>()
                .Include(x => x.Roulette)
                .Where(x => x.DeletedAt == null && x.ClosedAt == null && x.Id == roundId)
                .FirstOrDefaultAsync();

            if (round == null)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "round not found or closed!");

            return round;
        }

        private async Task<UserAccount> GetUserAccountOrAbortIfAuthUserIsNotAccountOwner(long userAccountId)
        {
            User user = await _identityApp.GetUser(AppDbContext);

            UserAccount userAccount = await AppDbContext.Set<UserAccount>()
                .Include(x => x.UserOwner)
                .Where(
                    x => x.DeletedAt == null &&
                    x.State.Id == (long)UserAccountStates.Active &&
                    x.UserOwner.Id == user.Id)
                .FirstOrDefaultAsync();

            if(userAccount == null)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "user account not found or not owner!");

            return userAccount;
        }

        private async Task AbortIfAccountHasInsufficientFundsOrInputAmountIsZero(UserAccount userAccount, decimal amount)
        {
            if (amount <= 0)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "the amount must be greater than zero");

            UserAccountBalance userAccountBalance = await (
                from a in AppDbContext.Set<UserAccount>()
                join va in AppDbContext.Set<UserAccountBalance>() on a.Id equals va.Id
                where
                    a.Id == userAccount.Id &&
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
                ).FirstOrDefaultAsync();

            if (userAccountBalance.AvailableBalance < amount)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "the account has insufficient funds!");
        }

        private async Task<RouletteRule> GetRouletteRuleOrAbortIfNotFound(BetCreateDTO betCreateDTO, Round round)
        {
            RouletteRule rouletteRule = await AppDbContext.Set<RouletteRule>()
                .Include(x => x.Roulette.Type)
                .Include(x => x.Roulette.State)
                .FirstOrDefaultAsync(
                    x => x.Id == betCreateDTO.RouletteRuleId &&
                    x.Roulette.Id == round.Roulette.Id &&
                    x.DeletedAt == null);

            if (rouletteRule == null)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "the rule not found!");

            return rouletteRule;
        }

        private async Task<AccountTransaction> GetAndSaveAccountTransaction(UserAccount userAccount, decimal amount)
        {
            AccountTransaction accountTransaction = new AccountTransaction
            {
                Amount = amount * -1,
                UserAccount = userAccount,
                UserRegister = await _identityApp.GetUser(AppDbContext),
                State = await AppDbContext
                    .FindGenericElementByIdAsync<AccountTransactionState>((long)AccountTransactionStates.Pending),
                Type = await AppDbContext
                    .FindGenericElementByIdAsync<AccountTransactionType>((long)AccountTransactionTypes.Bet)
            };

            AppDbContext.Set<AccountTransaction>().Add(accountTransaction);

            await AppDbContext.SaveChangesAsync();

            return accountTransaction;
        }

        private async Task SaveBetNumbersList(Bet bet, List<string> numbers)
        {
            if(numbers.Count > 0)
            {
                foreach (string strNumber in numbers)
                {
                    Number number = await AppDbContext.Set<Number>().FirstOrDefaultAsync(x => x.Name.Trim() == strNumber.Trim());

                    if (number == null)
                        throw new WebApiException(System.Net.HttpStatusCode.Forbidden, $"bet number '{strNumber}' is invalid!");

                    BetNumber betNumber = new BetNumber
                    {
                        Bet = bet,
                        Number = number
                    };

                    AppDbContext.Set<BetNumber>().Add(betNumber);
                }

                await AppDbContext.SaveChangesAsync();
            }
        }


        public async Task<ActionResult<WebApiResponse>> CancelBet(long roundId, long betId)
        {
            await GetRoundOrAbortIfRoundStatusClosed(roundId);
            
            Bet bet = await GetBetOrAbortIfNotFound(roundId, betId);

            await CancelBetAndAccountTransaction(bet);

            return MakeSuccessResponse(null, "bet canceled!");
        }

        private async Task<Bet> GetBetOrAbortIfNotFound(long roundId, long betId)
        {
            Bet bet = await AppDbContext.Set<Bet>()
                .Where(
                    x => x.Id == betId &&
                    x.Round.Id == roundId &&
                    x.State.Id == (long)BetStates.Active)
                .Include(x => x.AccountTransaction)
                .FirstOrDefaultAsync();

            if (bet == null)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "bet not found!");

            return bet;
        }

        private async Task CancelBetAndAccountTransaction(Bet bet)
        {
            bet.State = await AppDbContext.FindGenericElementByIdAsync<BetState>((long)BetStates.Canceled);

            AccountTransaction accountTransaction = bet.AccountTransaction;

            accountTransaction.State = await AppDbContext
                .FindGenericElementByIdAsync<AccountTransactionState>((long)AccountTransactionStates.Cancelled);

            await AppDbContext.BeginTransactionAsync();

            AppDbContext.Entry(bet).State = EntityState.Modified;
            AppDbContext.Entry(accountTransaction).State = EntityState.Modified;

            await AppDbContext.SaveChangesAsync();

            await AppDbContext.CommitTransactionAsync();
        }
    }
}
