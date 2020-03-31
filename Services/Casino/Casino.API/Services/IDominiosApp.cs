using Casino.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Services
{
    public interface IDominiosApp
    {
        public IEnumerable<Dominio> Dominios { get; }
        public DominiosAppNamesSingleton DominiosAppNames { get; }

        public void LoadDomains();
        public Task LoadDomainsAsync();

        public Dominio GetDominio(string name);
        public Dominio GetDominio(int id);

        public IEnumerable<Dominio> GetChildrenDomainsFromParentDomain(Dominio parent);
        public IEnumerable<Dominio> GetChildrenDomainsFromParentDomain(string parentName);
        public IEnumerable<Dominio> GetChildrenDomainsFromParentDomain(int parentId);

        public Dominio GetChildDomainFromParentDomain(int domainId, Dominio parentDomain);
        public Dominio GetChildDomainFromParentDomain(int domainId, string parentDomainName);
        public Dominio GetChildDomainFromParentDomain(int domainId, int parentDomainId);

        public Dominio GetChildDomainFromParentDomain(string domainName, Dominio parentDomain);
        public Dominio GetChildDomainFromParentDomain(string domainName, string parentDomainName);
        public Dominio GetChildDomainFromParentDomain(string domainName, int parentDomainId);
    }
}
