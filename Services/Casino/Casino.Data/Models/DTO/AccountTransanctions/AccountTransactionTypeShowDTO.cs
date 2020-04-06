using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.AccountTransanctions
{
    public class AccountTransactionTypeShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public string Type { get; set; }
    }
}
