using System.ComponentModel.DataAnnotations;

namespace Casino.API.Data.Models.User
{
    public class RoleDTO
    {
        [Required]
        public string Role { get; set; }
    }
}
