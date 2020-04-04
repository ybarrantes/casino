using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Casino.Services.WebApi;
using Casino.Services.DB.SQL.Crud;
using Casino.Data.Context;

namespace Casino.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/roulettes")]
    [Authorize(Policy = "Admin")]
    [Authorize(Policy = "SuperAdmin")]
    public class RoulettesController : ControllerBase
    {
        private readonly ISqlContextCrud<Roulette> _rouletteCrud;

        public RoulettesController(ApplicationDbContext dbContext, ISqlContextCrud<Roulette> contextCRUD)
        {
            _rouletteCrud = contextCRUD;
            _rouletteCrud.AppDbContext = dbContext;
        }


        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetAll(int page = 1)
        {
            return await _rouletteCrud.GetAllPagedRecordsAndResponseAsync(page, 10);
        }


        [HttpGet("{id}", Name = "GetRoulette")]
        public async Task<ActionResult<WebApiResponse>> GetOne(long id)
        {
            return await _rouletteCrud.FirstByIdAndResponseAsync(id);
        }


        [HttpPost]
        public async Task<ActionResult<WebApiResponse>> Create([FromBody] RouletteCreateDTO modelDTO)
        {
            return await _rouletteCrud.CreateFromModelDTOAndResponseAsync(modelDTO);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<WebApiResponse>> Update(long id, [FromBody] RouletteCreateDTO modelDTO)
        {
            return await _rouletteCrud.UpdateFromModelDTOAndResponseAsync(id, modelDTO);
        }        
        

        [HttpDelete("{id}")]
        public async Task<ActionResult<WebApiResponse>> Delete(long id)
        {
            return await _rouletteCrud.DeleteByIdAndResponseAsync(id);
        }

    }
}
