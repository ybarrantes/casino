using AutoMapper;
using Casino.API.Services;
using Casino.Data.Models.Default;
using Casino.Data.Models.DTO.Rounds;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Contracts;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Rounds
{
    public class RoundsCrudComponent : SqlContextCrud<Round>, IRoundComponent
    {
        private readonly ISqlContextCrud<Roulette> _rouletteCrudController;
        private readonly IIdentityApp<User> _identityApp;

        public RoundsCrudComponent(
            IMapper mapper,
            IPagedRecords<Round> pagedRecords,
            ISqlContextCrud<Roulette> rouletteContext,
            IIdentityApp<User> identityApp)
            : base(mapper, pagedRecords)
        {
            _identityApp = identityApp;

            ShowModelDTOType = typeof(RoundShowDTO);

            _rouletteCrudController = rouletteContext;
        }

        public override IPagedRecords<Round> MapPagedRecordsToModelDTO(IPagedRecords<Round> pagedRecords)
        {
            pagedRecords.Result = Mapper.Map<List<RoundShowDTO>>(pagedRecords.Result);

            return pagedRecords;
        }

        protected override async Task OnBeforeCreate(Round entity)
        {
            entity.UserOpen = await _identityApp.GetUser(AppDbContext);
        }

        public async Task<ActionResult<WebApiResponse>> GetAllRouletteRoundsPagedRecordsAsync(long rouletteId, int page)
        {
            await AbortOnRouletteNotExists(rouletteId);

            QueryFilter = new GetRoundsOfARouletteQueryFilter(rouletteId);

            return await GetAllPagedRecordsAndMakeResponseAsync(page, 20);
        }

        private async Task AbortOnRouletteNotExists(long rouletteId)
        {
            await GetRoulette(rouletteId);
        }

        public async Task<ActionResult<WebApiResponse>> OpenRouletteRoundAsync(long rouletteId)
        {
            Roulette roulette = await GetRoulette(rouletteId);

            CheckIfRouletteIsActive(roulette);

            CheckIfRouletteTypeIsSupported(roulette);

            await CheckIfExistsAnotherRoundOpen(roulette);

            Round round = new Round
            {
                Roulette = roulette,
                State = await GetRoundState(RoundStates.Opened)
            };

            round = await CreateFromEntityAsync(round);

            return MapEntityAndMakeResponse(round);
        }

        private async Task<Roulette> GetRoulette(long id)
        {
            _rouletteCrudController.AppDbContext = AppDbContext;

            return await _rouletteCrudController.FirstByIdAsync(id);
        }

        private void CheckIfRouletteIsActive(Roulette roulette)
        {
            if (roulette.State.Id != (long)RouletteStates.Active)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "the roulette state is not valid!");
        }

        private void CheckIfRouletteTypeIsSupported(Roulette roulette)
        {
            if (roulette.Type.Id != (long)RouletteTypes.European)
                throw new WebApiException(System.Net.HttpStatusCode.NotImplemented, "the roulette type not implemented!");
        }

        private async Task CheckIfExistsAnotherRoundOpen(Roulette roulette)
        {
            long openRounds = await AppDbContext.Set<Round>()
                .Where(x => x.ClosedAt == null && x.Roulette.Id.Equals(roulette.Id))
                .LongCountAsync();

            if (openRounds > 0)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "there is another round open");
        }

        private async Task<RoundState> GetRoundState(RoundStates state)
        {
            RoundState roundState = await AppDbContext
                .Set<RoundState>()
                .FirstOrDefaultAsync(x => x.Id == (long)state);

            if (roundState == null)
                throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, "round state not found!");

            return roundState;
        }

        public async Task<ActionResult<WebApiResponse>> CloseRouletteRoundAsync(long rouletteId, long roundId)
        {
            await AbortOnRouletteNotExists(rouletteId);

            Round round = await FirstByIdAsync(roundId);

            AbortOnClosedRoundState(round);

            round = await CloseRoundAndStartPlayRoulette(round);

            return MapEntityAndMakeResponse(round);
        }

        private void AbortOnClosedRoundState(Round round)
        {
            if (round.ClosedAt != null)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "the round is already closed");
        }

        private async Task<Round> CloseRoundAndStartPlayRoulette(Round round)
        {
            await AppDbContext.BeginTransactionAsync();

            round.State = await GetRoundState(RoundStates.Closed);
            round.ClosedAt = DateTime.Now;
            round.UserClose = await _identityApp.GetUser(AppDbContext);

            AppDbContext.Entry(round).State = EntityState.Modified;

            round = await UpdateFromEntityAsync(round);

            // TODO: launch process to get the winning number

            // TODO: launch process to collect bets

            await AppDbContext.CommitTransactionAsync();

            return round;
        }
    }
}
