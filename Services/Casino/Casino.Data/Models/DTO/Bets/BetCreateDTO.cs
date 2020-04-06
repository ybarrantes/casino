
using Casino.Services.DB.SQL.Contracts.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Casino.Data.Models.DTO.Bets
{
    public class BetCreateDTO : IModelDTO
    {
        [Required]
        public long AccountId {get; set;}

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public long RoundId { get; set; }

        [Required]
        public long RouletteRuleId { get; set; }

        [Required]
        public List<string> Numbers { get; set; }
    }
}
