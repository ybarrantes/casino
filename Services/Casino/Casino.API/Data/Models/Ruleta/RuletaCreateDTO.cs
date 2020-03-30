using Casino.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Models.Ruleta
{
    public class RuletaCreateDTO
    {
        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        public int Estado { get; set; }

        [Required]
        public int TipoRuleta { get; set; }
    }
}
