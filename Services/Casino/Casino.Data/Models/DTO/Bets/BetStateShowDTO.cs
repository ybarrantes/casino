using Casino.Data.Models.DTO.AccountTransanctions;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.DTO.Rounds;
using Casino.Services.DB.SQL.Contracts.Model;
using System;

namespace Casino.Data.Models.DTO.Bets
{
    public class BetStateShowDTO : IModelDTO
    {
        public string State { get; set; }
    }
}
