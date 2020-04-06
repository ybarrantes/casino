using Casino.Data.Models.Entities;
using Casino.Data.Models.Enums;
using Casino.Services.DB.SQL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.BusisnessLogic
{
    public class PlayRoulette
    {
        public ApplicationDbContextBase AppDbContext { get; set; }

        public async Task PlayRouletteAndApplyPays(Round round)
        {
            Roulette roulette = await GetRoulette(round);

            IEnumerable<RouletteTypeNumber> rouletteNumberList = await GetNumberListToPlay(roulette);

            Number winNumber = GetWinNumber(rouletteNumberList);

            round.WinNumber = winNumber;

            AppDbContext.Entry(round).State = EntityState.Modified;
            await AppDbContext.SaveChangesAsync();

            await ApplyPayments(round, winNumber);
        }

        private async Task<Roulette> GetRoulette(Round round)
        {
            return await AppDbContext.Set<Roulette>()
                .Where(x => x.Id == round.Roulette.Id)
                .FirstOrDefaultAsync();
        }

        private async Task<IEnumerable<RouletteTypeNumber>> GetNumberListToPlay(Roulette roulette)
        {
            return await AppDbContext.Set<RouletteTypeNumber>()
                .Include(x => x.Number)
                .Where(x => x.Type.Id == roulette.Type.Id)
                .ToListAsync();
        }

        private Number GetWinNumber(IEnumerable<RouletteTypeNumber> numberList)
        {
            var random = new Random();

            int index = random.Next(numberList.Count());

            return numberList.ElementAt(index).Number;
        }

        private async Task ApplyPayments(Round round, Number winNumber)
        {
            IEnumerable<Bet> roundBetList = await GetBetList(round, winNumber);

            if (roundBetList.Count() > 0)
            {
                AccountTransactionState accountTransactionAproved = await AppDbContext
                    .FindGenericElementByIdAsync<AccountTransactionState>((long)AccountTransactionStates.Approved);

                foreach(Bet bet in roundBetList)
                {
                    int count = bet.BetNumbers.Where(x => x.Number.Id == winNumber.Id).Count();

                    if (count > 0) // winner
                    {
                        decimal amount = Math.Abs(bet.AccountTransaction.Amount) * (1 + (decimal)bet.PaymentRatio);
                        AccountTransaction accountTransaction = new AccountTransaction
                        {
                            Amount = amount,
                            UserAccount = bet.AccountTransaction.UserAccount,
                            UserRegister = await AppDbContext.FindGenericElementByIdAsync<User>(1),
                            State = accountTransactionAproved,
                            Type = bet.AccountTransaction.Type
                        };

                        AppDbContext.Set<AccountTransaction>().Add(accountTransaction);
                    }

                    bet.AccountTransaction.State = accountTransactionAproved;

                    AppDbContext.Entry(bet.AccountTransaction).State = EntityState.Modified;
                }

                await AppDbContext.SaveChangesAsync();
            }
        }

        private async Task<IEnumerable<Bet>> GetBetList(Round round, Number winNumber)
        {
            return await AppDbContext.Set<Bet>()
                .Include(x => x.BetNumbers)
                    .ThenInclude(x => x.Number)
                .Include(x => x.AccountTransaction.State)
                .Include(x => x.AccountTransaction.Type)
                .Include(x => x.AccountTransaction.UserAccount)
                .Where(x => x.Round.Id == round.Id && x.State.Id == (long)BetStates.Active)
                .ToListAsync();
        }

        
    }
}
