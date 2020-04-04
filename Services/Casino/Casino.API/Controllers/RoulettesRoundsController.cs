
using Casino.API.Components;
using Casino.API.Components.Roulettes;
using Casino.API.Components.Rounds;
using Casino.Data.Context;
using Casino.Data.Models.Default;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/roulettes/{id}/rounds")]
    public class RoulettesRoundsController
    {
        private readonly ISqlContextCrud<Round> _roundCrudController;
        private readonly ISqlContextCrud<Roulette> _rouletteCrudController;

        public RoulettesRoundsController(
            ApplicationDbContext dbContext,
            ISqlContextCrud<Roulette> rouletteContext,
            ISqlContextCrud<Round> crudComponent)
        {
            _roundCrudController = crudComponent;
            _roundCrudController.AppDbContext = dbContext;

            _rouletteCrudController = rouletteContext;
            _rouletteCrudController.AppDbContext = dbContext;
            _rouletteCrudController.QueryableFilter = new RoulettesQueryableFilter();
        }

        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetAll(long id, int page = 1)
        {
            // check roulette exists
            await _rouletteCrudController.FirstById(id);

            _roundCrudController.QueryableFilter = new GetRoundsOfARouletteQueryFilter(id);

            return await _roundCrudController.GetAllPagedRecordsAndResponseAsync(page, 20);
        }

        [HttpPost("open")]
        public async Task<ActionResult<WebApiResponse>> OpenRound(long id)
        {
            // set filter to only get roulettes in active state
            _rouletteCrudController.QueryableFilter = new OnlyGetRoulettesActiveStateQueryFilter(id);

            Roulette roulette = await _rouletteCrudController.FirstById(id);

            CheckIfRouletteTypeIsSupported(roulette);

            CheckIfExistsAnotherRoundOpen(roulette.Rounds);

            Round round = new Round
            {
                Roulette = roulette,
                State = await GetRoundState(RoundStates.Opened)
            };

            round = await _roundCrudController.CreateFromEntityAsync(round);

            return _roundCrudController.MapEntityAndResponse(round);
        }

        private void CheckIfRouletteTypeIsSupported(Roulette roulette)
        {
            if (roulette.Type.Id != (long)RouletteTypes.European)
                throw new WebApiException(System.Net.HttpStatusCode.NotImplemented, "the roulette type not implemented!");
        }

        private void CheckIfExistsAnotherRoundOpen(List<Round> rounds)
        {
            int roundsOpened = rounds
                .Where(x => x.ClosedAt == null)
                .Count();

            if (roundsOpened > 0)
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, "there is another round open");
        }

        private async Task<RoundState> GetRoundState(RoundStates state)
        {
            RoundState roundState = await _roundCrudController.AppDbContext
                .Set<RoundState>()
                .FirstOrDefaultAsync(x => x.Id == (long)state);

            if(roundState == null)
                throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, "round state not found!");

            return roundState;
        }

        [HttpPost("{idRound}/close")]
        public async Task<ActionResult<WebApiResponse>> CloseRound(long id, long idRound)
        {
            // check roulette exsist
            await _rouletteCrudController.FirstById(id);

            Round round = await _roundCrudController.FirstById(idRound);

            // TODO: process calculate winners
            await ((ApplicationDbContext)_roundCrudController.AppDbContext).BeginTransactionAsync();

            round.State = await GetRoundState(RoundStates.Closed);
            round.ClosedAt = DateTime.Now;

            _roundCrudController.AppDbContext.Entry(round).State = EntityState.Modified;

            round = await _roundCrudController.UpdateFromEntityAsync(round);

            await ((ApplicationDbContext)_roundCrudController.AppDbContext).RollbackTransactionAsync();

            return _roundCrudController.MapEntityAndResponse(round);
        }
    }
}
