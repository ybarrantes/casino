using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Entities
{
    public class Usuario : ITimestamps, ISoftDeletes
    {
        protected DateTime _CreatedAt;
        protected DateTime _UpdatedAt;
        protected DateTime _DeletedAt;

        public long Id { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Apellidos { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        public long UsuarioRegistraId { get; set; }
        public string CloudIdentityId { get; set; }

        public DateTime CreatedAt { get => _CreatedAt; set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; set => _UpdatedAt = value; }
        public DateTime DeletedAt { get => _DeletedAt; set => _DeletedAt = value; }
    }
}
