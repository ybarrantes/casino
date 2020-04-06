using System;
using System.Collections.Generic;
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
        public RouletteState State { get; set; }

        [Required]
        public RouletteType Type { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        // relations
        public List<Round> Rounds { get; set; }

        public List<RouletteRule> Rules { get; set; }
    }
}
