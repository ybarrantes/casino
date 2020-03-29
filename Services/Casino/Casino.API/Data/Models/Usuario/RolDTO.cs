using Casino.API.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Models.Usuario
{
    public class RolDTO
    {
        [Required]
        public string Rol { get; set; }
    }
}
