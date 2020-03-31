using Casino.API.Data.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace Casino.API.Data.Models.Roulette
{
    public class RouletteShowDTO
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public DomainShowDTO State { get; set; }

        [Required]
        public DomainShowDTO Type { get; set; }
    }
}
