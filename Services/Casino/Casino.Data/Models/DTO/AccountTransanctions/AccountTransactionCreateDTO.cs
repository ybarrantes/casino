using Casino.Services.DB.SQL.Contracts.Model;
using System.ComponentModel.DataAnnotations;

namespace Casino.Data.Models.DTO.AccountTransanctions
{
    public class AccountTransactionCreateDTO : IModelDTO
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public long Type { get; set; }
    }
}
