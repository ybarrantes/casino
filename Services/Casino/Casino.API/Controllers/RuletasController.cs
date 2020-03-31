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
        private readonly IIdentityApp identityApp;

        public RuletasController(
            ApplicationDbContext dbContext,
            ILogger<RuletasController> logger,
            IMapper mapper,
            IIdentityApp identityApp)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            this.identityApp = identityApp;
        }


        [HttpGet]
        public async Task<ActionResult<Util.Response.HttpResponse>> GetRuletas(int page = 1)
        {
            IEnumerable<Ruleta> ruletas = await dbContext.Ruletas
                .Include(r => r.Estado)
                .Include(b => b.Tipo)
                .Where(x => x.DeletedAt == null)
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
            Ruleta ruleta = await CreateRuletaEntity(ruletaDTO);

            await TrySaveRuleta(ruleta);

            RuletaShowDTO ruletaGetDTO = mapper.Map<RuletaShowDTO>(ruleta);

            return new CreatedAtRouteResult("GetRuleta", new { id = ruleta.Id }, ruletaGetDTO);
        }

        private async Task<Ruleta> CreateRuletaEntity(RuletaCreateDTO ruletaDTO)
        {
            Ruleta ruleta = new Ruleta()
            {
                Descripcion = ruletaDTO.Descripcion,
                Estado = await GetAndValidateDominiosRuletaFromId(ruletaDTO.Estado, DominiosAppNamesSingleton.GetInstance.ESTADOS_RULETAS, "Estado"),
                Tipo = await GetAndValidateDominiosRuletaFromId(ruletaDTO.TipoRuleta, DominiosAppNamesSingleton.GetInstance.TIPOS_RULETAS, "Tipo"),
                UsuarioRegistraId = identityApp.GetUser(dbContext)
            };

            return ruleta;
        }

        private async Task<Dominio> GetAndValidateDominiosRuletaFromId(int idDominio, string nombreDominioPadre, string field)
        {
            Dominio padre = await dbContext.Dominios.FirstOrDefaultAsync<Dominio>(d => d.Nombre.Equals(nombreDominioPadre));

            if(padre == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, $"domain '{nombreDominioPadre}' not found!");

            Dominio dominio = await dbContext.Dominios.FirstOrDefaultAsync<Dominio>(d => d.Padre != null && d.Padre.Id.Equals(padre.Id) && d.Id.Equals(idDominio));

            if (dominio == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, $"{field}: value '{idDominio}' is invalid!'");

            return dominio;
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

            return new Util.Response.HttpResponse()
                .Success()
                .SetData(ruletaGetDTO)
                .SetMessage("deleted successful");
        }

        private bool RuletaExists(int id)
        {
            return dbContext.Ruletas.Any(e => e.Id == id);
        }
    }
}
