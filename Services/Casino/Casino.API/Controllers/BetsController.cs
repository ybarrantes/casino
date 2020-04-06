using Casino.API.Components.Bets;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Bets;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/rounds/{roundId}/bets")]
    public class BetsController : ControllerBase
    {
        private readonly ISqlContextCrud<Bet> _betContextCrud;

        public BetsController(
            ApplicationDbContext dbContext,
            ISqlContextCrud<Bet> betContextCrud)
        {
            _betContextCrud = betContextCrud;
            _betContextCrud.AppDbContext = dbContext;
        }

        [HttpGet("{betId}")]
        public async Task<ActionResult<WebApiResponse>> GetOneOpenedRound(long roundId, long betId)
        {
            IQueryable<Bet> query = _betContextCrud.GetQueryableWithFilter()
                .Include(x => x.Round.State)
                .Include(x => x.AccountTransaction.State)
                .Include(x => x.State)
                .Where(x => x.Id == betId && x.Round.Id == roundId);

            return await _betContextCrud.FirstFromQueryAndMakeResponseAsync(query);
        }

        [HttpPost]
        public async Task<ActionResult<WebApiResponse>> CreateBet(long roundId, [FromBody] BetCreateDTO betCreateDTO)
        {
            return await ((IBetComponent)_betContextCrud).CreateBet(roundId, betCreateDTO);
        }

        [HttpDelete("{betId}")]
        public async Task<ActionResult<WebApiResponse>> CancelBet(long roundId, long betId)
        {
            return await ((IBetComponent)_betContextCrud).CancelBet(roundId, betId);
        }
    }
}
