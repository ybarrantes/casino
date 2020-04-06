using Casino.Data.Models.DTO.Users;
using Casino.Services.DB.SQL.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Casino.Data.Models.DTO.AccountTransanctions
{
    public class AccountTransactionShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public UserShowDTO UserRegister { get; set; }

        public decimal Amount { get; set; }

        public AccountTransactionStateShowDTO State { get; set; }

        public AccountTransactionTypeShowDTO Type { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
