using Casino.API.Config;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Data.Queries
{
    public class InsertDefaultDomainsQuery
    {
        public static void addDomains(MigrationBuilder migrationBuilder)
        {
            Dictionary<string, List<string>> dictDomains = new Dictionary<string, List<string>> { };

            dictDomains.Add(ApiDomains.ESTADOS_RULETAS, new List<string> { "Activo", "Inactivo", "Suspendido" });
            dictDomains.Add(ApiDomains.TIPOS_RULETAS, new List<string> { "Americana", "Europea" });

            insertData(migrationBuilder, dictDomains);
        }

        private static void insertData(MigrationBuilder migrationBuilder, Dictionary<string, List<string>> dictDomains)
        {
            foreach (KeyValuePair<string, List<string>> row in dictDomains)
            {
                string parentValidation = $"SELECT Id FROM Dominios WHERE Nombre = '{row.Key}' AND PadreId IS NULL";
                migrationBuilder.Sql($"INSERT INTO Dominios (Nombre) SELECT '{row.Key}' WHERE NOT EXISTS({parentValidation});");

                foreach (string child in row.Value)
                {
                    migrationBuilder.Sql($"INSERT INTO Dominios (Nombre, PadreId) SELECT '{child}', ({parentValidation}) WHERE NOT EXISTS (SELECT Id FROM Dominios WHERE Nombre = '{child}' AND PadreId = ({parentValidation}));");
                }
            }
        }
    }
}
