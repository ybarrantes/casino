using System.ComponentModel.DataAnnotations;
using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO
{
    public class RouletteShowDTO : IModelDTO<Entities.Roulette>
    {
        public long Id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public DomainShowDTO State { get; set; }

        [Required]
        public DomainShowDTO Type { get; set; }


        #region IModelDTO<Roulette> Members

        public void FillDTOFromEntity(Entities.Roulette entity)
        {
            throw new System.NotImplementedException();
        }

        public Entities.Roulette FillEntityFromDTO()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
