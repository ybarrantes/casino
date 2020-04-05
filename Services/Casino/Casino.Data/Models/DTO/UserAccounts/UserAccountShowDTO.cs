using Casino.Data.Models.DTO.Rounds;
using Casino.Data.Models.DTO.Users;
using Casino.Services.DB.SQL.Contracts.Model;
using System;

namespace Casino.Data.Models.DTO.UserAccounts
{
    public class UserAccountShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public UserShowDTO UserOwner { get; set; }

        public UserAccountStateShowDTO State { get; set; }

        public UserAccountTypeShowDTO Type { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal BetBalance { get; set; }

        public decimal AvailableBalance { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
