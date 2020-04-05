using Casino.Services.DB.SQL.Contracts.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casino.Data.Models.Entities
{
    public class Bet : IEntityModelBase, IEntityModelTimestamps
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Round Round { get; set; }

        /*[Required]
        public AccountTransaction AccountTransaction { get; set; }*/

        [Required]
        public User UserRegister { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }
    }
}
