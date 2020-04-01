using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.Entities
{
    public class User : IEntityModelUser, IEntityModelTimestamps, IEntityModelSoftDeletes
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string CloudIdentityId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
