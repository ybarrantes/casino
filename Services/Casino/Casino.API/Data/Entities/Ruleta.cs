using Casino.API.Data.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Entities
{
    public class Ruleta : IApiEntityModel, ITimestampsEntityModel, ISoftDeletesEntityModel
    {
        protected DateTime? _CreatedAt;
        protected DateTime? _UpdatedAt;
        protected DateTime? _DeletedAt;

        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        public Usuario UsuarioRegistraId { get; set; }

        [Required]
        public Dominio Estado { get; set; }

        [Required]
        public Dominio Tipo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get => _CreatedAt; set => _CreatedAt = value; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get => _UpdatedAt; set => _UpdatedAt = value; }
        public DateTime? DeletedAt { get => _DeletedAt; set => _DeletedAt = value; }
    }
}
