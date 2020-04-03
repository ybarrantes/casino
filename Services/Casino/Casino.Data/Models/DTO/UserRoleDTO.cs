using System.ComponentModel.DataAnnotations;

namespace Casino.Data.Models.DTO
{
    public class UserRoleDTO
    {
        [Required]
        public string Role { get; set; }
    }
}
