using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Casino.Data.Context;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;
using Casino.Services.WebApi;
using Casino.API.Services;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Casino.Services.Util.Collections;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Casino.API.Controllers
{
    [Authorize]
    [Route("api/roulettes")]
    [ApiController]
    public class RoulettesController : ControllerBase
    {
        private readonly IContextCRUD<Roulette> _CRUD;
        private readonly ApplicationDbContext _dbContext;
        private readonly IIdentityApp<User> _identityApp;
        private readonly IMapper _mapper;

        public RoulettesController(
            ApplicationDbContext dbContext,
            IContextCRUD<Roulette> contextCRUD,
            IIdentityApp<User> identityApp,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _identityApp = identityApp;
            _mapper = mapper;

            _CRUD = contextCRUD;
            _CRUD.AppDbContext = dbContext;
        }


        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetRoulettes(int page = 1)
        {
            IPagedRecords pagedRecords = await _CRUD.FindAllPagedAsync(page, 10);

            List<RouletteShowDTO> mappedListDTO = _mapper.Map<List<RouletteShowDTO>>(pagedRecords.Result);

            return MakeSuccessResponse(mappedListDTO, "");
        }


        [HttpGet("{id}", Name = "GetRoulette")]
        public async Task<ActionResult<WebApiResponse>> GetRoulette(long id)
        {
            Roulette entity = await _CRUD.FindByIdAsync(id);

            return MapEntityAndMakeSuccessResponse(entity, "success!");
        }


        [HttpPost]
        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<WebApiResponse>> CreateRoulette([FromBody] RouletteCreateDTO rouletteDTO)
        {
            Roulette entity = await FillRouletteFromDTO(rouletteDTO);

            entity.UserRegister = await _identityApp.GetUser(_dbContext);

            entity = await _CRUD.CreateFromEntityAsync(entity);

            return MapEntityAndMakeSuccessResponse(entity, "success!");
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<WebApiResponse>> UpdateRoulette(int id, [FromBody] RouletteCreateDTO rouletteDTO)
        {
            await _CRUD.FindByIdAsync(id);

            Roulette entity = await FillRouletteFromDTO(rouletteDTO);
            entity.Id = id;

            entity = await _CRUD.UpdateFromEntityAsync(entity);

            return MapEntityAndMakeSuccessResponse(entity, "success!");
        }        
        

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<WebApiResponse>> DeleteRoulette(long id)
        {
            Roulette entity = await _CRUD.FindByIdAsync(id);

            entity = await _CRUD.DeleteByEntityAsync(entity);

            return MapEntityAndMakeSuccessResponse(entity, "success!");
        }


        private async Task<Roulette> FillRouletteFromDTO(RouletteCreateDTO dto)
        {
            return new Roulette()
            {
                Description = dto.Description,
                State = await GetRouletteStateAsync(dto.State),
                Type = await GetRouletteTypeAsync(dto.Type),
            };
        }

        private async Task<RouletteState> GetRouletteStateAsync(long id)
        {
            RouletteState rouletteState = await _dbContext.RouletteStates.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (rouletteState == null)
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, "roulette state is invalid");

            return rouletteState;
        }

        private async Task<RouletteType> GetRouletteTypeAsync(long id)
        {
            RouletteType rouletteType = await _dbContext.RouletteTypes.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (rouletteType == null)
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, "roulette type is invalid");

            return rouletteType;
        }


        private RouletteShowDTO MapEntityToModelDTO(Roulette entity)
        {
            return _mapper.Map<RouletteShowDTO>(entity);
        }

        public ActionResult<WebApiResponse> MakeSuccessResponse(object data, string message)
        {
            return new WebApiResponse()
                .Success()
                .SetData(data)
                .SetMessage(message);
        }

        public ActionResult<WebApiResponse> MapEntityAndMakeSuccessResponse(Roulette entity, string message)
        {
            RouletteShowDTO map = MapEntityToModelDTO(entity);

            return MakeSuccessResponse(map, message);
        }
    }
}
