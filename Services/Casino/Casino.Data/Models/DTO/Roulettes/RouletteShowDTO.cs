using System.ComponentModel.DataAnnotations;
using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.Roulettes
{
    public class RouletteShowDTO : IModelDTO
    {
        public long Id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public RouletteStateDTO State { get; set; }

        [Required]
        public RouletteTypeDTO Type { get; set; }
    }
}
