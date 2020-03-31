using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Casino.API.Data.Models.Roulette;
using AutoMapper;
using Casino.API.Services;
using Casino.API.Data.Extension;
using Casino.API.Components.Roulettes;

namespace Casino.API.Controllers
{
    [Authorize]
    [Route("api/roulettes")]
    [ApiController]
    public class RoulettesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RoulettesController> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityApp _identityApp;
        private readonly IRoulettesComponent _roulettesComponent;

        public RoulettesController(
            ApplicationDbContext dbContext,
            ILogger<RoulettesController> logger,
            IMapper mapper,
            IIdentityApp identityApp,
            IRoulettesComponent ruletasComponent)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _identityApp = identityApp;
            _roulettesComponent = ruletasComponent;

            _roulettesComponent.AppDbContext = _dbContext;
        }


        [HttpGet]
        public async Task<ActionResult<Util.Response.HttpResponse>> GetRoulettes(int page = 1)
        {
            PagedRecordsEntityModel pagedRecords = new PagedRecordsEntityModel(_roulettesComponent.QueryableRoulette(_dbContext.Roulettes), page, RoulettesComponent.RECORDS_PER_PAGE);
            await pagedRecords.Build();

            pagedRecords.Result = _mapper.Map<List<RouletteShowDTO>>(pagedRecords.Result);

            return MakeSuccessResponse(pagedRecords.Result, "");
        }

        private ActionResult<Util.Response.HttpResponse> MakeSuccessResponse(object data, string message)
        {
            return new Util.Response.HttpResponse()
                .Success()
                .SetData(data)
                .SetMessage(message);                
        }


        [HttpGet("{id}", Name = "GetRoulette")]
        public async Task<ActionResult<Util.Response.HttpResponse>> GetRoulette(int id)
        {
            Roulette roulette = await _roulettesComponent.FindRouletteById(id);

            return MakeRouletteDTOResponse(roulette, "");
        }

        private ActionResult<Util.Response.HttpResponse> MakeRouletteDTOResponse(Roulette roulette, string message)
        {
            RouletteShowDTO rouletteDTO = MapRouletteToDTO(roulette);

            return MakeSuccessResponse(rouletteDTO, message);
        }

        private RouletteShowDTO MapRouletteToDTO(Roulette roulette)
        {
            return _mapper.Map<RouletteShowDTO>(roulette);
        }        



        [HttpPost]
        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Util.Response.HttpResponse>> PostRoulette([FromBody] RouletteCreateDTO rouletteDTO)
        {
            Roulette roulette = await _roulettesComponent.CreateRoulette(rouletteDTO);

            return MakeRouletteDTOResponse(roulette, "Roulette created");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<Util.Response.HttpResponse>> PutRoulette(int id, [FromBody] RouletteCreateDTO rouletteDTO)
        {
            Roulette roulette = await _roulettesComponent.UpdateRouletteById(rouletteDTO, id);

            return MakeRouletteDTOResponse(roulette, "Roulette updated");
        }
        
        

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<Util.Response.HttpResponse>> DeleteRoulette(int id)
        {
            Roulette roulette = await _roulettesComponent.DeleteRouletteById(id);

            return MakeRouletteDTOResponse(roulette, "Roulette deleted");
        }
    }
}
