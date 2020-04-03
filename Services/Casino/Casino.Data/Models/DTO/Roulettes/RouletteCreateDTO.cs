using System.ComponentModel.DataAnnotations;
using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO.Roulettes
{
    public class RouletteCreateDTO : IModelDTO
    {
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public long State { get; set; }

        [Required]
        public long Type { get; set; }
    }
}
