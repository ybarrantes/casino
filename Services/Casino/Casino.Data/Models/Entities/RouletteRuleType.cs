using Casino.Services.DB.SQL.Contracts.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casino.Data.Models.Entities
{
    public class RouletteRuleType : IEntityModelBase, IEntityModelTimestamps
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public RouletteType Type { get; set; }

        public string Description { get; set; }

        [Required]
        public float DefaultPay { get; set; }

        [Required]
        public byte Numbers { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }
    }
}
