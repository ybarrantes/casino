using Casino.Data.Context;
using Casino.Data.Models.DTO.Roulettes;
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
    [Route("api/roulettes/states")]
    public class RoulettesStatesController : ControllerBase
    {
        private readonly ISqlContextCrud<RouletteState> _rouletteStateCrud;

        public RoulettesStatesController(ApplicationDbContext dbContext, ISqlContextCrud<RouletteState> contextCRUD)
        {
            _rouletteStateCrud = contextCRUD;
            _rouletteStateCrud.AppDbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetAll(int page = 1)
        {
            return await _rouletteStateCrud.GetAllPagedRecordsAndMakeResponseAsync(page, 10);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<WebApiResponse>> GetOne(long id)
        {
            return await _rouletteStateCrud.FirstByIdAndMakeResponseAsync(id);
        }


        [HttpPost]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> Create([FromBody] RouletteStateCreateDTO modelDTO)
        {
            return await _rouletteStateCrud.CreateFromModelDTOAndMakeResponseAsync(modelDTO);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> Update(long id, [FromBody] RouletteStateCreateDTO modelDTO)
        {
            return await _rouletteStateCrud.UpdateFromModelDTOAndMakeResponseAsync(id, modelDTO);
        }
    }
}
