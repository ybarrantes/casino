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
using System.Threading.Tasks;

namespace Casino.API.Components.Roulettes
{
    public class RoulettesTypesCRUDComponent : CRUDComponent<RouletteType>
    {
        public override Type ShowModelDTOType { get; internal set; }

        public RoulettesTypesCRUDComponent(
            ApplicationDbContext dbContext,
            ContextCRUD<RouletteType> contextCRUD,
            IIdentityApp<User> identityApp,
            IMapper mapper)
            : base(dbContext, contextCRUD, identityApp, mapper)
        {
            ShowModelDTOType = typeof(RouletteTypeDTO);
        }

        public override async Task<RouletteType> FillEntityFromDTO(RouletteType entity, IModelDTO modelDTO)
        {
            entity.Type = ((RouletteStateCreateDTO)modelDTO).State;

            return await Task.Run(() => entity);
        }

        public override IPagedRecords MapPagedRecordsToModelDTO(IPagedRecords pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<RouletteTypeDTO>>(pagedRecords.Result);

            return pagedRecords;
        }
    }
}
