using Microsoft.EntityFrameworkCore;
using Casino.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Casino.API.Data.Extension;
using Casino.API.Exceptions;

namespace Casino.API.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Dominio> Dominios { get; set; }
        public DbSet<Ruleta> Ruletas { get; set; }



        /// <summary>
        /// Add support feature ITimestampsEntityModel, ISoftDeletesEntityModel
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            string _DeletedAtColumnName = "DeletedAt";
            string _CreatedAtColumnName = "CreatedAt";
            string _UpdatedAtColumnName = "UpdatedAt";

            try
            {
                // created at
                var entitiesAdded = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added && e.Metadata.GetProperties()
                        .Any(x => x.Name == _CreatedAtColumnName))
                    .ToList();

                foreach (var entity in entitiesAdded)
                {
                    entity.CurrentValues[_CreatedAtColumnName] = DateTime.Now;
                    entity.CurrentValues[_UpdatedAtColumnName] = DateTime.Now;
                }
            }
            catch (Exception)
            {
            }


            try
            {
                // updated at
                var entitiesModified = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified && e.Metadata.GetProperties()
                        .Any(x => x.Name == _UpdatedAtColumnName))
                    .ToList();

                foreach (var entity in entitiesModified)
                {
                    entity.CurrentValues[_UpdatedAtColumnName] = DateTime.Now;
                }
            }
            catch (Exception)
            {
            }


            try
            {
                // soft deletes
                var entitiesDeleted = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Deleted && e.Metadata.GetProperties()
                        .Any(x => x.Name == _DeletedAtColumnName))
                    .ToList();

                foreach (var entity in entitiesDeleted)
                {
                    entity.State = EntityState.Unchanged;
                    entity.CurrentValues[_DeletedAtColumnName] = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, "soft deletes failed: " + e.Message);
            }

            return base.SaveChanges();
        }
    }
}
