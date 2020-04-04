
using Casino.API.Components.Rounds;
using Casino.Data.Context;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Casino.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/roulettes/{rouletteId}/rounds")]
    public class RoulettesRoundsController
    {
        private readonly ISqlContextCrud<Round> _roundCrudComponent;
        
        public RoulettesRoundsController(
            ApplicationDbContext dbContext,
            ISqlContextCrud<Round> crudComponent)
        {
            _roundCrudComponent = crudComponent;
            _roundCrudComponent.AppDbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetAll(long rouletteId, int page = 1)
        {
            return await ((IRoundComponent)_roundCrudComponent)
                .GetAllRouletteRoundsPagedRecordsAsync(rouletteId, page);
        }

        [HttpPost("open")]
        public async Task<ActionResult<WebApiResponse>> OpenRound(long rouletteId)
        {
            return await ((IRoundComponent)_roundCrudComponent).OpenRouletteRoundAsync(rouletteId);
        }        

        [HttpPost("{roundId}/close")]
        public async Task<ActionResult<WebApiResponse>> CloseRound(long rouletteId, long roundId)
        {
            return await ((IRoundComponent)_roundCrudComponent).CloseRouletteRoundAsync(rouletteId, roundId);
        }
    }
}
