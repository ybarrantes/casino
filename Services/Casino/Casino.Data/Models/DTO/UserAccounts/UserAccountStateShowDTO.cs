using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.UserAccounts
{
    public class UserAccountStateShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public string State { get; set; }
    }
}
