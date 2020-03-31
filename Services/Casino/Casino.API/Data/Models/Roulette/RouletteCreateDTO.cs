using System.ComponentModel.DataAnnotations;

namespace Casino.API.Data.Models.Roulette
{
    public class RouletteCreateDTO
    {
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public int State { get; set; }

        [Required]
        public int Type { get; set; }
    }
}
