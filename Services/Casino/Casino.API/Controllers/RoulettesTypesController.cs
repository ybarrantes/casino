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
    [Route("api/roulettes-types")]
    public class RoulettesTypesController : ControllerBase
    {
        private readonly ISqlContextCrud<RouletteType> _rouletteTypeCrud;

        public RoulettesTypesController(ApplicationDbContext dbContext, ISqlContextCrud<RouletteType> contextCRUD)
        {
            _rouletteTypeCrud = contextCRUD;
            _rouletteTypeCrud.AppDbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> GetAll(int page = 1)
        {
            return await _rouletteTypeCrud.GetAllPagedRecordsAndMakeResponseAsync(page, 10);
        }


        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> GetOne(long id)
        {
            return await _rouletteTypeCrud.FirstByIdAndMakeResponseAsync(id);
        }


        [HttpPost]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> Create([FromBody] RouletteTypeCreateDTO modelDTO)
        {
            return await _rouletteTypeCrud.CreateFromModelDTOAndMakeResponseAsync(modelDTO);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> Update(long id, [FromBody] RouletteTypeCreateDTO modelDTO)
        {
            return await _rouletteTypeCrud.UpdateFromModelDTOAndMakeResponseAsync(id, modelDTO);
        }
    }
}
