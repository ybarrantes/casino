using Casino.Services.DB.SQL.Contracts.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casino.Data.Models.Entities
{
    public class BetNumber : IEntityModelBase
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Bet Bet { get; set; }

        [Required]
        public Number Number { get; set; }
    }
}

