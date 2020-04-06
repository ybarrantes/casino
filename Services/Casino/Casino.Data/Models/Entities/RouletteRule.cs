using Casino.Services.DB.SQL.Contracts.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casino.Data.Models.Entities
{
    public class RouletteRule : IEntityModelBase, IEntityModelTimestamps, IEntityModelSoftDeletes
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Roulette Roulette { get; set; }

        [Required]
        public RouletteRuleType Type { get; set; }

        [Required]
        public float Pay { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}

