using System.ComponentModel.DataAnnotations;

namespace Casino.Data.Models.DTO.Users
{
    public class UserRoleDTO
    {
        [Required]
        public string Role { get; set; }
    }
}
