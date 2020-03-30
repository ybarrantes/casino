using Casino.API.Data.Entities;
using Casino.API.Data.Models.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Models.Ruleta
{
    public class RuletaShowDTO
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        public DominioShowDTO Estado { get; set; }

        [Required]
        public DominioShowDTO Tipo { get; set; }
    }
}
