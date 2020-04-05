using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casino.Data.Models.Views
{
    [Table("UserAccountsBalance")]
    public class UserAccountBalance : IEntityModelBase
    {
        [Key]
        public long Id { get; set; }

        public User UserOwner { get; set; }

        public UserAccountState State { get; set; }

        public UserAccountType Type { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal BetBalance { get; set; }

        public decimal AvailableBalance { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}

