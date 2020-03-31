using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Casino.API.Config;
using Microsoft.Extensions.Configuration;

namespace Casino.API.Services
{
    public class DominiosAppServiceSingleton : IDominiosApp, IDisposable
    {
        private static DominiosAppServiceSingleton _instance = null;
        private IEnumerable<Dominio> _dominios = null;
        private DominiosAppNamesSingleton _dominiosAppNames;
        private readonly ApplicationDbContext _dbContext;


        public IEnumerable<Dominio> Dominios => _dominios;
        public DominiosAppNamesSingleton DominiosAppNames => _dominiosAppNames;


        public DominiosAppServiceSingleton(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dominiosAppNames = new DominiosAppNamesSingleton();
        }


        // singleton
        public async static Task<DominiosAppServiceSingleton> GetInstance(ApplicationDbContext dbContext)
        {
            if (_instance == null)
            {
                _instance = new DominiosAppServiceSingleton(dbContext);
                await _instance.LoadDomainsAsync();
            }

            return _instance;
        }

        public void LoadDomains()
        {
            _dominios = _dbContext.Dominios.Include(d => d.Padre).ToList();
        }

        public async Task LoadDomainsAsync()
        {
            _dominios = await _dbContext.Dominios.Include(d => d.Padre).ToListAsync();
        }


        #region Obtener dominios por nombre o id

        public Dominio GetDominio(string name)
        {
            Dominio dominio = Dominios.FirstOrDefault(d => !String.IsNullOrEmpty(d.Nombre) && d.Nombre.Equals(name));
            return CheckDomainNullReferenceException(dominio, name);
        }

        public Dominio GetDominio(int id)
        {
            Dominio dominio = Dominios.FirstOrDefault(d => d.Id.Equals(id));
            return CheckDomainNullReferenceException(dominio, id.ToString());
        }

        private Dominio CheckDomainNullReferenceException(Dominio dominio, string flag)
        {
            if (dominio == null)
                throw new NullReferenceException($"domain: {flag} not found!");

            return dominio;
        }


        #endregion



        #region obtener los dominios hijos recibiendo algun valor del dominio padre

        public IEnumerable<Dominio> GetChildrenDomainsFromParentDomain(string parentDomainName)
        {
            Dominio parent = GetDominio(parentDomainName);
            return GetChildrenDomainsFromParentDomain(parent);
        }

        public IEnumerable<Dominio> GetChildrenDomainsFromParentDomain(int parentId)
        {
            Dominio parent = GetDominio(parentId);
            return GetChildrenDomainsFromParentDomain(parent);
        }

        public IEnumerable<Dominio> GetChildrenDomainsFromParentDomain(Dominio parent)
        {
            return Dominios
                .Where<Dominio>(d => d.Padre != null && d.Padre.Id.Equals(parent.Id))
                .ToList();
        }

        #endregion


        #region obtener un dominio hijo (especificando id) y recibiendo algun valor del dominio padre

        public Dominio GetChildDomainFromParentDomain(int domainId, Dominio parentDomain)
        {
            return GetChildrenDomainsFromParentDomain(parentDomain)
                .FirstOrDefault(d => d.Id.Equals(domainId));
        }

        public Dominio GetChildDomainFromParentDomain(int domainId, string parentDomainName)
        {
            return GetChildrenDomainsFromParentDomain(parentDomainName)
                .FirstOrDefault(d => d.Id.Equals(domainId));
        }

        public Dominio GetChildDomainFromParentDomain(int domainId, int parentDomainId)
        {
            return GetChildrenDomainsFromParentDomain(parentDomainId)
                .FirstOrDefault(d => d.Id.Equals(domainId));
        }

        #endregion


        #region obtener un dominio hijo (especificando nombre) y recibiendo algun valor del dominio padre

        public Dominio GetChildDomainFromParentDomain(string domainName, Dominio parentDomain)
        {
            return GetChildrenDomainsFromParentDomain(parentDomain)
                .FirstOrDefault(d => !String.IsNullOrEmpty(d.Nombre) && d.Nombre.Equals(domainName));
        }

        public Dominio GetChildDomainFromParentDomain(string domainName, string parentDomainName)
        {
            return GetChildrenDomainsFromParentDomain(parentDomainName)
                .FirstOrDefault(d => !String.IsNullOrEmpty(d.Nombre) && d.Nombre.Equals(domainName));
        }

        public Dominio GetChildDomainFromParentDomain(string domainName, int parentDomainId)
        {
            return GetChildrenDomainsFromParentDomain(parentDomainId)
                .FirstOrDefault(d => !String.IsNullOrEmpty(d.Nombre) && d.Nombre.Equals(domainName));
        }

        #endregion

        public void Dispose()
        {
            _dominios = null;
        }
    }
}
