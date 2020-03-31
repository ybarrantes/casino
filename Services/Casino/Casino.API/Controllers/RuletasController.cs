using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Casino.API.Exceptions;
using Casino.API.Data.Models.Ruleta;
using AutoMapper;
using Casino.API.Config;
using Casino.API.Services;
using Casino.API.Data.Extension;
using Casino.API.Components.Ruletas;

namespace Casino.API.Controllers
{
    [Authorize]
    [Route("api/ruletas")]
    [ApiController]
    public class RuletasController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RuletasController> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityApp _identityApp;
        private readonly IRuletasComponent _ruletasComponent;

        public RuletasController(
            ApplicationDbContext dbContext,
            ILogger<RuletasController> logger,
            IMapper mapper,
            IIdentityApp identityApp,
            IRuletasComponent ruletasComponent)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _identityApp = identityApp;
            _ruletasComponent = ruletasComponent;

            _ruletasComponent.AppDbContext = _dbContext;
        }


        [HttpGet]
        public async Task<ActionResult<Util.Response.HttpResponse>> GetRuletas(int page = 1)
        {
            PagedRecordsEntityModel pagedRecords = new PagedRecordsEntityModel(_ruletasComponent.QueryableRuleta(_dbContext.Ruletas), page, RuletasComponent.RECORDS_PER_PAGE);
            await pagedRecords.Build();

            pagedRecords.Result = _mapper.Map<List<RuletaShowDTO>>(pagedRecords.Result);

            return new Util.Response.HttpResponse().Success().SetData(pagedRecords);
        }


        [HttpGet("{id}", Name = "GetRuleta")]
        public async Task<ActionResult<Util.Response.HttpResponse>> GetRuleta(int id)
        {
            Ruleta ruleta = await _ruletasComponent.FindRuletaById(id);
            RuletaShowDTO ruletaDTO = _mapper.Map<RuletaShowDTO>(ruleta);

            return new Util.Response.HttpResponse().Success().SetData(ruletaDTO);
        }




        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Util.Response.HttpResponse>> PostRuleta([FromBody] RuletaCreateDTO ruletaDTO)
        {
            Ruleta ruleta = await _ruletasComponent.CreateRuleta(ruletaDTO);

            return RedirectToGetRuleta(ruleta);
        }

        private CreatedAtRouteResult RedirectToGetRuleta(Ruleta ruleta)
        {
            RuletaShowDTO ruletaGetDTO = _mapper.Map<RuletaShowDTO>(ruleta);
            return new CreatedAtRouteResult("GetRuleta", new { id = ruleta.Id }, ruletaGetDTO);
        }


        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Util.Response.HttpResponse>> PutRuleta(int id, [FromBody] RuletaCreateDTO ruletaDTO)
        {
            Ruleta ruleta = await _ruletasComponent.UpdateRuletaById(ruletaDTO, id);

            return RedirectToGetRuleta(ruleta);
        }
        
        

        // DELETE: api/Ruletas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Util.Response.HttpResponse>> DeleteRuleta(int id)
        {
            Ruleta ruleta = await _ruletasComponent.DeleteRuletaById(id);

            RuletaShowDTO ruletaGetDTO = _mapper.Map<RuletaShowDTO>(ruleta);

            return new Util.Response.HttpResponse()
                .Success()
                .SetData(ruletaGetDTO)
                .SetMessage("deleted successful");
        }
    }
}
