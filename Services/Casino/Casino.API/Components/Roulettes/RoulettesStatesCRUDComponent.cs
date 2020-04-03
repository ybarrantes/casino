using AutoMapper;
using Casino.API.Services;
using Casino.Data.Context;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.Util.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Roulettes
{
    public class RoulettesStatesCRUDComponent : CRUDComponent<RouletteState>
    {
        public override Type ShowModelDTOType { get; internal set; }

        public RoulettesStatesCRUDComponent(ApplicationDbContext dbContext, ContextCRUD<RouletteState> contextCRUD, IIdentityApp<User> identityApp, IMapper mapper)
            : base(dbContext, contextCRUD, identityApp, mapper)
        {
            ShowModelDTOType = typeof(RouletteStateDTO);
        }

        public override async Task<RouletteState> FillEntityFromDTO(RouletteState entity, IModelDTO modelDTO)
        {
            entity.State = ((RouletteStateCreateDTO)modelDTO).State;

            return await Task.Run(() => entity);
        }

        public override IPagedRecords MapPagedRecordsToModelDTO(IPagedRecords pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<RouletteStateDTO>>(pagedRecords.Result);

            return pagedRecords;
        }
    }
}
