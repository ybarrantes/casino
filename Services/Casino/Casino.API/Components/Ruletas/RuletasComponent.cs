using Casino.API.Config;
using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Casino.API.Data.Models.Ruleta;
using Casino.API.Exceptions;
using Casino.API.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Ruletas
{
    public class RuletasComponent : IRuletasComponent
    {
        public const int RECORDS_PER_PAGE = 15;

        private ApplicationDbContext _appDbContext = null;
        private readonly IIdentityApp _identityApp;

        public ApplicationDbContext AppDbContext
        {
            get
            {
                if (_appDbContext == null)
                    throw new InvalidOperationException("Undefined AppDbContext");

                return _appDbContext;
            }
            set => _appDbContext = value;
        }

        public RuletasComponent(IIdentityApp identityApp)
        {
            _identityApp = identityApp;
        }



        public async Task<Ruleta> FindRuletaById(int id)
        {
            Ruleta ruleta = await QueryableRuleta(AppDbContext.Ruletas)
                .Where(r => r.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (ruleta == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"ruleta with id '{id}' not found!");

            return ruleta;
        }

        /// <summary>
        /// Aplica filtro generico de consulta DB a las ruletas
        /// </summary>
        /// <param name="query">DataSet de consulta</param>
        /// <returns>Objeto de consulta con filtros predeterminados aplicados</returns>
        public IQueryable<Ruleta> QueryableRuleta(DbSet<Ruleta> query)
        {
            return query
                .Include(r => r.Estado)
                .Include(b => b.Tipo)
                .Where(x => x.DeletedAt == null);
        }

        

        public async Task<Ruleta> CreateRuleta(RuletaCreateDTO ruletaDTO)
        {
            Ruleta ruleta = await FillRuletaFromDTO(new Ruleta(), ruletaDTO);

            await TrySaveRuleta(ruleta, EntityState.Added);

            return ruleta;
        }

        public async Task<Ruleta> FillRuletaFromDTO(Ruleta ruleta, RuletaCreateDTO ruletaDTO)
        {
            ruleta.Descripcion = ruletaDTO.Descripcion;
            ruleta.Estado = await GetAndValidateDominiosRuleta(ruletaDTO.Estado, DominiosAppNamesSingleton.GetInstance.ESTADOS_RULETAS, "Estado");
            ruleta.Tipo = await GetAndValidateDominiosRuleta(ruletaDTO.TipoRuleta, DominiosAppNamesSingleton.GetInstance.TIPOS_RULETAS, "Tipo");
            ruleta.UsuarioRegistraId = _identityApp.GetUser(AppDbContext);

            return ruleta;
        }

        public async Task<Dominio> GetAndValidateDominiosRuleta(int dominioId, string nombreDominioPadre, string field)
        {
            Dominio padre = await AppDbContext.Dominios.FirstOrDefaultAsync<Dominio>(d => d.Nombre.Equals(nombreDominioPadre));

            if (padre == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"domain '{nombreDominioPadre}' not found!");

            Dominio dominio = await AppDbContext.Dominios.FirstOrDefaultAsync<Dominio>(d => d.Padre != null && d.Padre.Id.Equals(padre.Id) && d.Id.Equals(dominioId));

            if (dominio == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, $"{field}: value '{dominioId}' is invalid!'");

            return dominio;
        }

        public async Task TrySaveRuleta(Ruleta ruleta, EntityState entityOperation)
        {
            try
            {
                switch (entityOperation)
                {
                    case EntityState.Added:
                        AppDbContext.Ruletas.Add(ruleta);
                        break;
                    case EntityState.Modified:
                        AppDbContext.Entry(ruleta).State = EntityState.Modified;
                        break;
                    case EntityState.Deleted:
                        AppDbContext.Ruletas.Remove(ruleta);
                        break;
                }

                await AppDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
        }



        public async Task<Ruleta> UpdateRuletaById(RuletaCreateDTO ruletaDTO, int id)
        {
            Ruleta ruleta = await FindRuletaById(id);

            ruleta = await FillRuletaFromDTO(ruleta, ruletaDTO);

            await TrySaveRuleta(ruleta, EntityState.Modified);

            return ruleta;
        }



        public async Task<Ruleta> DeleteRuletaById(int id)
        {
            Ruleta ruleta = await FindRuletaById(id);

            await TrySaveRuleta(ruleta, EntityState.Deleted);

            return ruleta;
        }
    }
}
