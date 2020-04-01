using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Casino.Data.Context;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;
using Casino.Services.WebApi;
using Casino.API.Components;

namespace Casino.API.Controllers
{
    [Authorize]
    [Route("api/roulettes")]
    [ApiController]
    public class RoulettesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICRUDController<Roulette> _CRUDComponent;

        public RoulettesController(
            ApplicationDbContext dbContext,
            ICRUDController<Roulette> CRUDComponent)
        {
            _dbContext = dbContext;
            _CRUDComponent = CRUDComponent;

            _CRUDComponent.AppDbContext = _dbContext;
        }


        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetRoulettes(int page = 1)
        {
            return await _CRUDComponent.FindAllAndResponseAsync(page, 10);
        }


        [HttpGet("{id}", Name = "GetRoulette")]
        public async Task<ActionResult<WebApiResponse>> GetRoulette(long id)
        {
            return await _CRUDComponent.FindAndResponseAsync(id);
        }


        [HttpPost]
        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<WebApiResponse>> PostRoulette([FromBody] RouletteCreateDTO rouletteDTO)
        {
            return await _CRUDComponent.CreateAndResponseAsync(rouletteDTO);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<WebApiResponse>> PutRoulette(int id, [FromBody] RouletteCreateDTO rouletteDTO)
        {
            return await _CRUDComponent.UpdateAndResponseAsync(id, rouletteDTO);
        }        
        

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<WebApiResponse>> DeleteRoulette(long id)
        {
            return await _CRUDComponent.DeleteAndResponseAsync(id);
        }
    }
}
