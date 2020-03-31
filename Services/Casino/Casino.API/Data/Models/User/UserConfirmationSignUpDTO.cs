using System.ComponentModel.DataAnnotations;

namespace Casino.API.Data.Models.User
{
    public class UserConfirmationSignUpDTO
    {
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 4)]
        public string ConfirmationCode { get; set; }
    }
}
