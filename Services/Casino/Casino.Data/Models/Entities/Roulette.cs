using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.Entities
{
    public class Roulette : IEntityModelBase, IEntityModelTimestamps, IEntityModelSoftDeletes
    {
        [Key]
        public long Id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public User UserRegister { get; set; }

        [Required]
        public Domain State { get; set; }

        [Required]
        public Domain Type { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
