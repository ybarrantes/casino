﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Casino.API.Data.Models.Usuario
{
    public class UsuarioSignInDTO
    {
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
