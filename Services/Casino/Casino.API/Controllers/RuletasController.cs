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
            IEnumerable<Ruleta> ruletas = await QueryableRuleta(dbContext.Ruletas)                
                .ToListAsync();

            List<RuletaShowDTO> ruletasDTO = mapper.Map<List<RuletaShowDTO>>(ruletas);

            return new Util.Response.HttpResponse().Success().SetData(ruletasDTO);
        }

        /// <summary>
        /// Constructor de consultas de la ruleta
        /// </summary>
        /// <param name="query">Data set ruleta</param>
        /// <returns></returns>
        private IQueryable<Ruleta> QueryableRuleta(DbSet<Ruleta> query)
        {
            return query
                .Include(r => r.Estado)
                .Include(b => b.Tipo)
                .Where(x => x.DeletedAt == null);
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
            Ruleta ruleta = await QueryableRuleta(dbContext.Ruletas)
                .FirstOrDefaultAsync();

            if (ruleta == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"ruleta with id '{id}' not found!");

            return ruleta;
        }




        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Util.Response.HttpResponse>> PostRuleta([FromBody] RuletaCreateDTO ruletaDTO)
        {
            Ruleta ruleta = await CreateRuletaEntity(ruletaDTO);

            await TryCreateRuleta(ruleta);

            return RedirectToGetRuleta(ruleta);
        }

        private async Task<Ruleta> CreateRuletaEntity(RuletaCreateDTO ruletaDTO)
        {
            Ruleta ruleta = new Ruleta()
            {
                Descripcion = ruletaDTO.Descripcion,
                Estado = await GetAndValidateDominiosRuleta(ruletaDTO.Estado, DominiosAppNamesSingleton.GetInstance.ESTADOS_RULETAS, "Estado"),
                Tipo = await GetAndValidateDominiosRuleta(ruletaDTO.TipoRuleta, DominiosAppNamesSingleton.GetInstance.TIPOS_RULETAS, "Tipo"),
                UsuarioRegistraId = identityApp.GetUser(dbContext)
            };

            return ruleta;
        }

        private async Task<Dominio> GetAndValidateDominiosRuleta(int dominioId, string nombreDominioPadre, string field)
        {
            Dominio padre = await dbContext.Dominios.FirstOrDefaultAsync<Dominio>(d => d.Nombre.Equals(nombreDominioPadre));

            if (padre == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"domain '{nombreDominioPadre}' not found!");

            Dominio dominio = await dbContext.Dominios.FirstOrDefaultAsync<Dominio>(d => d.Padre != null && d.Padre.Id.Equals(padre.Id) && d.Id.Equals(dominioId));

            if (dominio == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, $"{field}: value '{dominioId}' is invalid!'");

            return dominio;
        }

        private async Task TryCreateRuleta(Ruleta ruleta)
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

        private CreatedAtRouteResult RedirectToGetRuleta(Ruleta ruleta)
        {
            RuletaShowDTO ruletaGetDTO = mapper.Map<RuletaShowDTO>(ruleta);
            return new CreatedAtRouteResult("GetRuleta", new { id = ruleta.Id }, ruletaGetDTO);
        }


        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Util.Response.HttpResponse>> PutRuleta(int id, [FromBody] RuletaCreateDTO ruletaDTO)
        {
            // check if ruleta exists
            await FindRuletaById(id);

            Ruleta ruleta = await CreateRuletaEntity(ruletaDTO);
            ruleta.Id = id;

            await TryUpdateRuleta(ruleta);

            return RedirectToGetRuleta(ruleta);
        }

        private async Task TryUpdateRuleta(Ruleta ruleta)
        {
            try
            {
                dbContext.Entry(ruleta).State = EntityState.Modified;
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
    }
}
