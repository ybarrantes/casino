using Casino.Services.DB.SQL.Contracts.Model;
using System.ComponentModel.DataAnnotations;

namespace Casino.Data.Models.DTO.Rounds
{
    public class UserAccountTypeShowDTO : IModelDTO
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string Type { get; set; }
    }
}
