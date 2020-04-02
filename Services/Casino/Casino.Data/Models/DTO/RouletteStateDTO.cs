using Casino.Services.DB.SQL.Contracts.Model;
using System.ComponentModel.DataAnnotations;

namespace Casino.Data.Models.DTO
{

    public class RouletteStateDTO : IModelDTO
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string State { get; set; }
    }
}
