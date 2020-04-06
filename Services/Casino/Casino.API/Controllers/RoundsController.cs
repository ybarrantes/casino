
using Casino.API.Components.Rounds;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Rounds;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/rounds")]
    public class RoundsController : ControllerBase
    {
        private readonly ISqlContextCrud<Round> _roundContextCrud;

        public RoundsController(
            ApplicationDbContext dbContext,
            ISqlContextCrud<Round> roundContextCrud)
        {
            _roundContextCrud = roundContextCrud;
            _roundContextCrud.AppDbContext = dbContext;

            _roundContextCrud.ShowModelDTOType = typeof(RoundForBetsShowDTO);
            _roundContextCrud.QueryFilter = new GetRoundsOrderByDescending();
        }

        [HttpGet("active")]
        public async Task<ActionResult<WebApiResponse>> GetAllOpenedRounds(int page = 1)
        {
            _roundContextCrud.QueryFilter = new OnlyRoundsActiveQueryFilter();

            return await GetPagedResponse(page);
        }

        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetAllRounds(int page = 1)
        {
            return await GetPagedResponse(page);
        }

        private async Task<ActionResult<WebApiResponse>> GetPagedResponse(int page)
        {
            IPagedRecords<Round> pagedRecords = await _roundContextCrud.GetPagedRecordsAsync(
                _roundContextCrud.GetQueryableWithFilter(), page, 10);

            pagedRecords.Result = _roundContextCrud.Mapper.Map<List<RoundForBetsShowDTO>>(pagedRecords.Result);

            return _roundContextCrud.MakeSuccessResponse(pagedRecords);
        }

        [HttpGet("{roundId}")]
        public async Task<ActionResult<WebApiResponse>> GetOneOpenedRound(long roundId)
        {
            return await _roundContextCrud.FirstByIdAndMakeResponseAsync(roundId);
        }
    }
}
