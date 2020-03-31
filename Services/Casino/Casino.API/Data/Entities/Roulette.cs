using Casino.API.Data.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Entities
{
    public class Roulette : IApiEntityModel, ITimestampsEntityModel, ISoftDeletesEntityModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public User UserRegister { get; set; }

        [Required]
        public Domain State { get; set; }

        [Required]
        public Domain Type { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
