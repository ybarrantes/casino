using Casino.Data.Models.DTO.AccountTransanctions;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.DTO.Rounds;
using Casino.Services.DB.SQL.Contracts.Model;
using System;

namespace Casino.Data.Models.DTO.Bets
{
    public class BetShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public RoundShowDTO Round { get; set; }

        public AccountTransactionShowDTO AccountTransaction { get; set; }

        public RouletteRulesShowDTO RouletteRule { get; set; }

        public float PaymentRatio { get; set; }

        public BetStateShowDTO State { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
