using AutoMapper;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.API.Components.Roulettes
{
    public class RoulettesTypesCrudComponent : SqlContextCrud<RouletteType>
    {
        public RoulettesTypesCrudComponent(
            IMapper mapper,
            IPagedRecords<RouletteType> pagedRecords)
            : base(mapper, pagedRecords)
        {
            ShowModelDTOType = typeof(RouletteTypeDTO);
        }

        public override async Task<RouletteType> FillEntityFromModelDTO(RouletteType entity, IModelDTO modelDTO)
        {
            entity.Type = ((RouletteTypeCreateDTO)modelDTO).Type;

            return await Task.Run(() => entity);
        }

        public override IPagedRecords<RouletteType> MapPagedRecordsToModelDTO(IPagedRecords<RouletteType> pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<RouletteTypeDTO>>(pagedRecords.Result);

            return pagedRecords;
        }
    }
}
