using Casino.API.Components;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/roulettes/types")]
    public class RoulettesTypesController : ControllerBase
    {
        private readonly ISqlContextCrud<RouletteType> _rouletteTypeCrud;

        public RoulettesTypesController(ApplicationDbContext dbContext, ISqlContextCrud<RouletteType> contextCRUD)
        {
            _rouletteTypeCrud = contextCRUD;
            _rouletteTypeCrud.AppDbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetAll(int page = 1)
        {
            IQueryable<RouletteType> queryable = _rouletteTypeCrud.GetQueryable();

            IPagedRecords<RouletteType> paged = await _rouletteTypeCrud.GetPagedRecordsMapped(queryable, page, 10);

            return _rouletteTypeCrud.MakeSuccessResponse(paged);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<WebApiResponse>> GetOne(long id)
        {
            return await _rouletteTypeCrud.FirstByIdAndResponseAsync(id);
        }


        [HttpPost]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> Create([FromBody] RouletteTypeCreateDTO modelDTO)
        {
            return await _rouletteTypeCrud.CreateFromModelDTOAndResponseAsync(modelDTO);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> Update(long id, [FromBody] RouletteTypeCreateDTO modelDTO)
        {
            return await _rouletteTypeCrud.UpdateFromModelDTOAndResponseAsync(id, modelDTO);
        }
    }
}
