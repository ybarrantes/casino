
using AutoMapper;
using Casino.API.Services;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Rounds;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.Util.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.API.Components.Rounds
{
    public class RoundsCRUDComponent : CRUDComponent<Round>
    {
        public override Type ShowModelDTOType { get; internal set; }

        public RoundsCRUDComponent(
            ApplicationDbContext dbContext,
            ContextCRUD<Round> contextCRUD,
            IIdentityApp<User> identityApp,
            IMapper mapper)
            : base(dbContext, contextCRUD, identityApp, mapper)
        {
            ShowModelDTOType = typeof(RoundShowDTO);
        }

        public async override Task<Round> FillEntityFromDTO(Round entity, IModelDTO modelDTO)
        {
            entity.State = ((RoundShowDTO)modelDTO).State;

            return await Task.Run(() => entity);
        }

        public override IPagedRecords MapPagedRecordsToModelDTO(IPagedRecords pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<RoundShowDTO>>(pagedRecords.Result);

            return pagedRecords;
        }
    }
}
