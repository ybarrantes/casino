using Casino.API.Components;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Casino.API.Controllers
{
    [Authorize]
    [Route("api/roulettes/types")]
    [ApiController]
    public class RoulettesTypesController : ControllerBase
    {
        private readonly CRUDComponent<RouletteType> _CRUD;

        public RoulettesTypesController(CRUDComponent<RouletteType> contextCRUD)
        {
            _CRUD = contextCRUD;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> GetAll(int page = 1)
        {
            return await _CRUD.FindAllAndResponseAsync(page, 10);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<WebApiResponse>> GetOne(long id)
        {
            return await _CRUD.FindFromIdAndResponseAsync(id);
        }


        [HttpPost]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> Create([FromBody] RouletteTypeCreateDTO modelDTO)
        {
            return await _CRUD.CreateFromModelDTOAndResponseAsync(modelDTO);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> Update(long id, [FromBody] RouletteTypeCreateDTO modelDTO)
        {
            return await _CRUD.UpdateFromModelDTOAndResponseAsync(id, modelDTO);
        }
    }
}
