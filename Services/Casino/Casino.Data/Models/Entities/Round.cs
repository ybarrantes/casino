using Casino.Services.DB.SQL.Contracts.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casino.Data.Models.Entities
{
    public class Round : IEntityModelBase, IEntityModelTimestamps, IEntityModelSoftDeletes
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Roulette Roulette { get; set; }

        [Required]
        public RoundState State { get; set; }

        [StringLength(2)]
        public string WinNumber { get; set; }

        public DateTime? CloseAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
