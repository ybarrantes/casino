using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.Users
{
    public class UserShowDTO : IModelDTO
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
