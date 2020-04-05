using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.UserAccounts
{
    public class AccountTransactionCreateDTO : IModelDTO
    {
        public decimal Amount { get; set; }

        public long Type { get; set; }
    }
}
