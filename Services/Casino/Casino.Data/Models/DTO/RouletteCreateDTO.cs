using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.WebApi;
using Microsoft.EntityFrameworkCore;

namespace Casino.Data.Models.DTO
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
