using Casino.API.Data.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Entities
{
    public class Dominio : IApiEntityModel, ITimestampsEntityModel
    {
        protected DateTime? _CreatedAt;
        protected DateTime? _UpdatedAt;

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Nombre { get; set; }


        [StringLength(200)]
        public string Descripcion { get; set; }

        public Dominio Padre { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get => _CreatedAt; set => _CreatedAt = value; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get => _UpdatedAt; set => _UpdatedAt = value; }
    }

}
