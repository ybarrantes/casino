using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.AccountTransanctions
{
    public class AccountTransactionStateShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public string State { get; set; }
    }
}
