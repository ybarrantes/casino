using Casino.Services.DB.SQL.Contracts.Model;
using System.ComponentModel.DataAnnotations;

namespace Casino.Data.Models.DTO
{

    public class RouletteTypeDTO : IModelDTO
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string Type { get; set; }
    }
}