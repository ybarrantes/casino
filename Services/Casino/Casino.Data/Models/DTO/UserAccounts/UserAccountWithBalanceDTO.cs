using Casino.Data.Models.DTO.Rounds;
using Casino.Data.Models.DTO.Users;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.Model;
using System;

namespace Casino.Data.Models.DTO.UserAccounts
{
    public class UserAccountWithBalanceDTO : IModelDTO
    {
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
