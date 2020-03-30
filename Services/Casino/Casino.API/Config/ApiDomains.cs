using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casino.API.Data.Queries;

namespace Casino.API.Config
{
    // TODO: Crear como dependencia de tipo scope o singleton para evitar impactar performance por DB
    public class ApiDomains
    {
        public const string ESTADOS_RULETAS = "ESTADOS_RULETAS";
        public const string TIPOS_RULETAS = "TIPOS_RULETAS";

        private static Dictionary<string, IEnumerable<Dominio>> domainsDictionary = new Dictionary<string, IEnumerable<Dominio>>();

        public static IEnumerable<Dominio> GetChildDomains(ApplicationDbContext dbContext, string domainParentName)
        {
            IEnumerable<Dominio> list = null;

            if (!domainsDictionary.TryGetValue(domainParentName, out list))
            {
                Dominio parent = dbContext.Dominios
                    .Where<Dominio>(d => d.Nombre.Equals(domainParentName))
                    .FirstOrDefault();

                list = dbContext.Dominios.
                    Where<Dominio>(d => d.Padre.Id.Equals(parent.Id))
                    .ToList<Dominio>();

                domainsDictionary.Add(domainParentName, list);
            }

            return list;
        }

        public static Dominio ValidateIfDomainIdExistsInChildDomains(ApplicationDbContext dbContext, string domainParentName, int id)
        {
            IEnumerable<Dominio> list = GetChildDomains(dbContext, domainParentName);

            return list.Where<Dominio>(d => d.Id.Equals(id)).FirstOrDefault();
        }
    }
}
