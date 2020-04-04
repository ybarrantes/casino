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
    public class RoulettesStatesCrudComponent : SqlContextCrud<RouletteState>
    {
        public RoulettesStatesCrudComponent(
            IMapper mapper,
            IPagedRecords<RouletteState> pagedRecords)
            : base(mapper, pagedRecords)
        {
            ShowModelDTOType = typeof(RouletteStateDTO);
        }

        public override async Task<RouletteState> FillEntityFromModelDTO(RouletteState entity, IModelDTO modelDTO)
        {
            entity.State = ((RouletteStateCreateDTO)modelDTO).State;

            return await Task.Run(() => entity);
        }

        public override IPagedRecords<RouletteState> MapPagedRecordsToModelDTO(IPagedRecords<RouletteState> pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<RouletteStateDTO>>(pagedRecords.Result);

            return pagedRecords;
        }
    }
}
