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
using Casino.API.Util.Response;
using Casino.API.Exceptions;
using Casino.API.Data.Models.Ruleta;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Casino.API.Config;

namespace Casino.API.Controllers
{
    [Authorize]
    [Route("api/ruletas")]
    [ApiController]
    public class RuletasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<RuletasController> logger;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;

        public RuletasController(ApplicationDbContext dbContext, ILogger<RuletasController> logger, IMapper mapper, IHttpContextAccessor httpContext)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            this.httpContext = httpContext;
        }


        [HttpGet]
        public async Task<ActionResult<Util.Response.HttpResponse>> GetRuletas(int page = 1)
        {
            IEnumerable<Ruleta> ruletas = await dbContext.Ruletas
                .Include(r => r.Estado)
                .Include(b => b.Tipo)
                .ToListAsync();

            List<RuletaShowDTO> ruletasDTO = mapper.Map<List<RuletaShowDTO>>(ruletas);

            return new Util.Response.HttpResponse().Success().SetData(ruletasDTO);
        }


        [HttpGet("{id}", Name = "GetRuleta")]
        public async Task<ActionResult<Util.Response.HttpResponse>> GetRuleta(int id)
        {
            Ruleta ruleta = await FindRuletaById(id);
            RuletaShowDTO ruletaDTO = mapper.Map<RuletaShowDTO>(ruleta);

            return new Util.Response.HttpResponse().Success().SetData(ruletaDTO);
        }

        private async Task<Ruleta> FindRuletaById(int id)
        {
            Ruleta ruleta = await dbContext.Ruletas
                .Where(b => b.Id.Equals(id))
                .Include(b => b.Estado)
                .Include(b => b.Tipo)
                .FirstOrDefaultAsync();

            if (ruleta == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"ruleta with id '{id}' not found!");
            }

            return ruleta;
        }


        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRuleta(int id, Ruleta ruleta)
        {
            if (id != ruleta.Id)
            {
                return BadRequest();
            }

            dbContext.Entry(ruleta).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RuletaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Util.Response.HttpResponse>> PostRuleta(RuletaCreateDTO ruletaDTO)
        {
            Dominio dominiosEstadoRuleta = ApiDomains.ValidateIfDomainIdExistsInChildDomains(dbContext, ApiDomains.ESTADOS_RULETAS, ruletaDTO.Estado);
            if (dominiosEstadoRuleta == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, "Estado is not valid!");

            Dominio dominiosTipoRuleta = ApiDomains.ValidateIfDomainIdExistsInChildDomains(dbContext, ApiDomains.TIPOS_RULETAS, ruletaDTO.TipoRuleta);
            if (dominiosTipoRuleta == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, "TipoRuleta is not valid!");

            Ruleta ruleta = new Ruleta()
            {
                Descripcion = ruletaDTO.Descripcion,
                Estado = dominiosEstadoRuleta,
                Tipo = dominiosTipoRuleta,
                UsuarioRegistraId = ApiAuthUser.Singleton(httpContext, dbContext).User
            };

            await TrySaveRuleta(ruleta);

            RuletaShowDTO ruletaGetDTO = mapper.Map<RuletaShowDTO>(ruleta);

            return new CreatedAtRouteResult("GetRuleta", new { id = ruleta.Id }, ruletaGetDTO);
        }

        private async Task TrySaveRuleta(Ruleta ruleta)
        {
            try
            {
                dbContext.Ruletas.Add(ruleta);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
        }

        // DELETE: api/Ruletas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Util.Response.HttpResponse>> DeleteRuleta(int id)
        {
            Ruleta ruleta = await FindRuletaById(id);

            dbContext.Ruletas.Remove(ruleta);
            await dbContext.SaveChangesAsync();

            RuletaShowDTO ruletaGetDTO = mapper.Map<RuletaShowDTO>(ruleta);

            return new Util.Response.HttpResponse().Success().SetData(ruletaGetDTO).SetMessage("deleted successful");
        }

        private bool RuletaExists(int id)
        {
            return dbContext.Ruletas.Any(e => e.Id == id);
        }
    }
}
