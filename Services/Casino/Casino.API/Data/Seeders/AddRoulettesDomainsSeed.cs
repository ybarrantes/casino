using Casino.API.Config;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Casino.API.Data.Seeders
{
    public class AddRoulettesDomainsSeed
    {
        public static void AddDomains(MigrationBuilder migrationBuilder)
        {
            Dictionary<string, List<string>> dictDomains = new Dictionary<string, List<string>> { };

            dictDomains.Add(DominiosAppNamesSingleton.GetInstance.ROULETTES_STATES, new List<string> { "Active", "Inactive", "Suspended" });
            dictDomains.Add(DominiosAppNamesSingleton.GetInstance.ROULETTES_TYPES, new List<string> { "American", "European" });

            InsertData(migrationBuilder, dictDomains);
        }

        private static void InsertData(MigrationBuilder migrationBuilder, Dictionary<string, List<string>> dictDomains)
        {
            string domainsTableName = "Domains";
            string domainsTableFieldName = "Name";
            string domainsTableFielParentDomainId = "ParentDomainId";

            foreach (KeyValuePair<string, List<string>> row in dictDomains)
            {
                string parentValidation = $"SELECT Id FROM {domainsTableName} WHERE {domainsTableFieldName} = '{row.Key}' AND {domainsTableFielParentDomainId} IS NULL";
                migrationBuilder.Sql($"INSERT INTO {domainsTableName} ({domainsTableFieldName}) SELECT '{row.Key}' WHERE NOT EXISTS({parentValidation});");

                foreach (string child in row.Value)
                {
                    migrationBuilder.Sql(
                        $"INSERT INTO {domainsTableName} ({domainsTableFieldName}, {domainsTableFielParentDomainId}) " +
                        $"SELECT '{child}', ({parentValidation}) " +
                        $"WHERE NOT EXISTS (SELECT Id FROM {domainsTableName}" +
                        $"    WHERE {domainsTableFieldName} = '{child}' AND {domainsTableFielParentDomainId} = ({parentValidation}));");
                }
            }
        }
    }
}
