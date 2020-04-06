using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.UserAccounts
{
    public class UserAccountTypeShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public string Type { get; set; }
    }
}
