using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;
using Casino.Services.WebApi;
using Casino.API.Components;

namespace Casino.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/roulettes")]
    public class RoulettesController : ControllerBase
    {
        private readonly CRUDComponent<Roulette> _CRUD;

        public RoulettesController(CRUDComponent<Roulette> contextCRUD)
        {
            _CRUD = contextCRUD;
        }


        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetAll(int page = 1)
        {
            return await _CRUD.FindAllAndResponseAsync(page, 10);
        }


        [HttpGet("{id}", Name = "GetRoulette")]
        public async Task<ActionResult<WebApiResponse>> GetOne(long id)
        {
            return await _CRUD.FindFromIdAndResponseAsync(id);
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<WebApiResponse>> Create([FromBody] RouletteCreateDTO modelDTO)
        {
            return await _CRUD.CreateFromModelDTOAndResponseAsync(modelDTO);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<WebApiResponse>> Update(long id, [FromBody] RouletteCreateDTO modelDTO)
        {
            return await _CRUD.UpdateFromModelDTOAndResponseAsync(id, modelDTO);
        }        
        

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<WebApiResponse>> Delete(long id)
        {
            return await _CRUD.DeleteFromIdAndResponseAsync(id);
        }

    }
}
