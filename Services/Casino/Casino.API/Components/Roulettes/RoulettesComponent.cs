using Casino.API.Config;
using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Casino.API.Data.Models.Roulette;
using Casino.API.Exceptions;
using Casino.API.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Roulettes
{
    public class RoulettesComponent : IRoulettesComponent
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

        public RoulettesComponent(IIdentityApp identityApp)
        {
            _identityApp = identityApp;
        }



        public async Task<Roulette> FindRouletteById(int id)
        {
            Roulette roueltte = await QueryableRoulette(AppDbContext.Roulettes)
                .Where(r => r.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (roueltte == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"Roulette id '{id}' not found!");

            return roueltte;
        }

        public IQueryable<Roulette> QueryableRoulette(DbSet<Roulette> query)
        {
            return query
                .Include(r => r.State)
                .Include(b => b.Type)
                .Where(x => x.DeletedAt == null);
        }

        

        public async Task<Roulette> CreateRoulette(RouletteCreateDTO rouletteDTO)
        {
            Roulette roulette = await FillRouletteFromDTO(new Roulette(), rouletteDTO);

            AppDbContext.Roulettes.Add(roulette);

            await TrySaveRoulette();

            return roulette;
        }

        public async Task<Roulette> FillRouletteFromDTO(Roulette roulette, RouletteCreateDTO rouletteDTO)
        {
            roulette.Description = rouletteDTO.Description;
            roulette.State = await GetAndValidateDominiosRoulette(rouletteDTO.State, DominiosAppNamesSingleton.GetInstance.ROULETTES_STATES);
            roulette.Type = await GetAndValidateDominiosRoulette(rouletteDTO.Type, DominiosAppNamesSingleton.GetInstance.ROULETTES_TYPES);
            roulette.UserRegister = _identityApp.GetUser(AppDbContext);

            return roulette;
        }

        public async Task<Domain> GetAndValidateDominiosRoulette(int domainId, string parentDomainName)
        {
            Domain parentDomain = await AppDbContext.Domains.FirstOrDefaultAsync<Domain>(d => d.Name.Equals(parentDomainName));

            if (parentDomain == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"Domain '{parentDomainName}' not found!");

            Domain domain = await AppDbContext.Domains.FirstOrDefaultAsync<Domain>(d =>
                d.ParentDomain != null && d.ParentDomain.Id.Equals(parentDomain.Id) && d.Id.Equals(domainId));

            if (domain == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, $"domain value '{domainId}' is invalid!'");

            return domain;
        }

        public async Task TrySaveRoulette()
        {
            try
            {
                await AppDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
        }



        public async Task<Roulette> UpdateRouletteById(RouletteCreateDTO ruletaDTO, int id)
        {
            Roulette roulette = await FindRouletteById(id);

            roulette = await FillRouletteFromDTO(roulette, ruletaDTO);

            AppDbContext.Entry(roulette).State = EntityState.Modified;

            await TrySaveRoulette();

            return roulette;
        }



        public async Task<Roulette> DeleteRouletteById(int id)
        {
            Roulette roulette = await FindRouletteById(id);

            AppDbContext.Roulettes.Remove(roulette);

            await TrySaveRoulette();

            return roulette;
        }
    }
}
