using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Models.DTO
{
#nullable enable
    public class DomainShowDTO : IModelDTO<Entities.Domain>
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }


        #region IModelDTO<Domain> Members

        public void FillDTOFromEntity(Entities.Domain entity)
        {
            throw new System.NotImplementedException();
        }

        public Entities.Domain FillEntityFromDTO()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
