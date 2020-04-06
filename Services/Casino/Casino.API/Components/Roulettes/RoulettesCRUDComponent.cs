using AutoMapper;
using Casino.API.Services;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Casino.Data.Models.Enums;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Roulettes
{
    public class RoulettesCrudComponent : SqlContextCrud<Roulette>
    {
        private readonly IIdentityApp<User> _identityApp;

        public RoulettesCrudComponent(
            IIdentityApp<User> identityApp,
            IMapper mapper,
            IPagedRecords<Roulette> pagedRecords)
            : base(mapper, pagedRecords)
        {
            QueryFilter = new RoulettesQueryableFilter();

            _identityApp = identityApp;

            ShowModelDTOType = typeof(RouletteShowDTO);
        }

        public override async Task<Roulette> FillEntityFromModelDTO(Roulette entity, IModelDTO dto)
        {
            RouletteCreateDTO modelDto = (RouletteCreateDTO)dto;

            entity.Description = modelDto.Description;
            entity.State = await GetAndValidateRouletteProperties<RouletteState>(modelDto.State);
            entity.Type = await GetAndValidateRouletteProperties<RouletteType>(modelDto.Type);

            return entity;
        }

        public async Task<T> GetAndValidateRouletteProperties<T>(long id) where T : class
        {
            var entity = await AppDbContext
                .Set<T>()
                .FirstOrDefaultAsync(x => ((IEntityModelBase)x).Id.Equals(id));

            if (entity == null)
            {
                string entityClassName = typeof(T).Name;
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, $"{entityClassName} is invalid");
            }

            return entity;
        }

        public override IPagedRecords<Roulette> MapPagedRecordsToModelDTO(IPagedRecords<Roulette> pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<RouletteShowDTO>>(pagedRecords.Result);

            return pagedRecords;
        }

        protected override async Task OnBeforeCreate(Roulette entity)
        {
            entity.UserRegister = await _identityApp.GetUser(AppDbContext);
        }

        protected async override Task OnAfterCreate(Roulette entity)
        {
            await InsertRouletteDefaultRules(entity);
        }

        private async Task InsertRouletteDefaultRules(Roulette entity)
        {
            IEnumerable<RouletteRuleType> rouletteRuleTypeList = await AppDbContext.Set<RouletteRuleType>()
                .Where(x => x.Type.Id == entity.Type.Id)
                .ToListAsync();

            foreach (RouletteRuleType rouletteRuleType in rouletteRuleTypeList)
            {
                AppDbContext.Set<RouletteRule>().Add(
                    new RouletteRule
                    {
                        Roulette = entity,
                        Type = rouletteRuleType,
                        Pay = rouletteRuleType.DefaultPay
                    }
                    );
            }

            await AppDbContext.SaveChangesAsync();
        }

        protected override async Task OnBeforeUpdate(Roulette entity)
        {
            await RouletteRoundsConditionsOrAbort(entity);

            // TODO: if roulette type change, delete roulette rules and add default rules for new type
        }

        private async Task RouletteRoundsConditionsOrAbort(Roulette entity)
        {
            if (await RouletteHasRoundsAsync(entity))
            {
                // get current entity to check properties changes
                Roulette currentRoulette = await FirstByIdAsync(entity.Id);

                AbortUpdateOnRouletteTypeChange(currentRoulette, entity);

                await AbortUpdateOnRouletteStateChangeAndHasOpenRounds(currentRoulette, entity);
            }
        }        

        private async Task<bool> RouletteHasRoundsAsync(Roulette entity, RoundStates? roundState = null)
        {
            IQueryable<Round> query = AppDbContext.Set<Round>()
                .Where(x => x.Roulette.Id.Equals(entity.Id));

            if (roundState == RoundStates.Opened)
                query = query.Where(x => x.ClosedAt == null);
            else if(roundState == RoundStates.Closed)
                query = query.Where(x => x.ClosedAt != null);

            return (await query.LongCountAsync()) > 0;
        }  
        
        private void AbortUpdateOnRouletteTypeChange(Roulette currentRoulette, Roulette newRoulette)
        {
            if (currentRoulette.Type.Id != newRoulette.Type.Id)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden,
                    "the roulette has rounds, it's not possible to change the type field");
        }
        
        private async Task AbortUpdateOnRouletteStateChangeAndHasOpenRounds(Roulette currentRoulette, Roulette newRoulette)
        {
            if (currentRoulette.State.Id != newRoulette.State.Id)
            {
                if (await RouletteHasRoundsAsync(newRoulette, RoundStates.Opened))
                    throw new WebApiException(System.Net.HttpStatusCode.Forbidden,
                        "the roulette has open rounds, it's not possible to change the type state");
            }
        }
    }
}
