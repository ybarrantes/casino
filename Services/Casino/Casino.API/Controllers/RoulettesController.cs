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
using Casino.API.Components;

namespace Casino.API.Controllers
{
    [Authorize]
    [Route("api/roulettes")]
    [ApiController]
    public class RoulettesController : ControllerBase
    {
        private readonly ICRUDComponent<Roulette> _CRUD;

        public RoulettesController(
            ICRUDComponent<Roulette> contextCRUD)
        {
            _CRUD = contextCRUD;
        }


        [HttpGet]
        public async Task<ActionResult<WebApiResponse>> GetRoulettes(int page = 1)
        {
            return await _CRUD.FindAllAndResponseAsync(page, 10);
        }


        [HttpGet("{id}", Name = "GetRoulette")]
        public async Task<ActionResult<WebApiResponse>> GetRoulette(long id)
        {
            return await _CRUD.FindFromIdAndResponseAsync(id);
        }


        [HttpPost]
        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<WebApiResponse>> CreateRoulette([FromBody] RouletteCreateDTO rouletteDTO)
        {
            return await _CRUD.CreateFromModelDTOAndResponseAsync(rouletteDTO);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<WebApiResponse>> UpdateRoulette(long id, [FromBody] RouletteCreateDTO rouletteDTO)
        {
            return await _CRUD.UpdateFromModelDTOAndResponseAsync(id, rouletteDTO);
        }        
        

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ActionResult<WebApiResponse>> DeleteRoulette(long id)
        {
            return await _CRUD.DeleteFromIdAndResponseAsync(id);
        }

    }
}
