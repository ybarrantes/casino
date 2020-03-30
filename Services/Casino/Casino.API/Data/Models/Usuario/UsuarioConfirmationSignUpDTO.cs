using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Models.Usuario
{
    public class UsuarioConfirmationSignUpDTO
    {
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 4)]
        public string ConfirmationCode { get; set; }
    }
}
