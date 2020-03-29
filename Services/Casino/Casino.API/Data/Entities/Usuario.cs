using Casino.API.Data.Entities.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Entities
{
    public class Usuario : ITimestamps, ISoftDeletes
    {
        protected DateTime _CreatedAt;
        protected DateTime _UpdatedAt;
        protected DateTime? _DeletedAt;

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
        public DateTime CreatedAt { get => _CreatedAt; set => _CreatedAt = value; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get => _UpdatedAt; set => _UpdatedAt = value; }
        public DateTime? DeletedAt { get => _DeletedAt; set => _DeletedAt = value; }
    }
}
