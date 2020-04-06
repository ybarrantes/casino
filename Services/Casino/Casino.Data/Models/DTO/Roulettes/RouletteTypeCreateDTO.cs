using Casino.Services.DB.SQL.Contracts.Model;
using System.ComponentModel.DataAnnotations;

namespace Casino.Data.Models.DTO.Roulettes
{
    public class RouletteTypeCreateDTO : IModelDTO
    {
        [Required]
        [StringLength(100)]
        public string Type { get; set; }
    }
}
