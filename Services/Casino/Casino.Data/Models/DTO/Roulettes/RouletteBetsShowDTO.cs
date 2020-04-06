using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.Roulettes
{
    public class RouletteBetsShowDTO : IModelDTO
    {
        public long Id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public RouletteStateDTO State { get; set; }

        [Required]
        public RouletteTypeDTO Type { get; set; }

        public List<RouletteRulesShowDTO> Rules { get; set; }
    }
}
