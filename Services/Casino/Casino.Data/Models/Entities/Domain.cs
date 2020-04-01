using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.Entities
{
    public class Domain : IEntityModelBase, IEntityModelTimestamps
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }


        [StringLength(200)]
        public string Description { get; set; }

        public Domain ParentDomain { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }
    }

}
