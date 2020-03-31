using System.ComponentModel.DataAnnotations;

namespace Casino.API.Data.Models.User
{
    public class UserSignInDTO
    {
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
