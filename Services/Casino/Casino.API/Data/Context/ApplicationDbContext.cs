using Microsoft.EntityFrameworkCore;
using Casino.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casino.API.Data.Extension;
using System.Threading;

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



        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            BeforeSavingPrepareEntries(EntityState.Added, EntityState.Added, new List<string> { "CreatedAt", "UpdatedAt" });
            
            BeforeSavingPrepareEntries(EntityState.Modified, EntityState.Modified, new List<string> { "UpdatedAt" });

            BeforeSavingPrepareEntries(EntityState.Deleted, EntityState.Modified, new List<string> { "DeletedAt" });
        }


        /// <summary>
        /// Add support feature ITimestampsEntityModel, ISoftDeletesEntityModel
        /// </summary>
        /// <param name="_state">Estado actual de la entidad</param>
        /// <param name="_newState">Nuevo estado a asignar</param>
        /// <param name="columnNames">Columnas a modificar la marca de fecha y hora</param>
        private void BeforeSavingPrepareEntries(EntityState _state, EntityState _newState, List<string> columnNames)
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.State.Equals(_state) && e.Metadata.GetProperties()
                    .Any(x => columnNames.Contains(x.Name)))
                .ToList();

            foreach (var entity in entities)
            {
                foreach(string columnName in columnNames)
                {
                    bool changeValue = true;
                    if (entity.State.Equals(EntityState.Deleted))
                    {
                        changeValue = (columnName.Equals("DeletedAt") && entity.CurrentValues[columnName] == null);
                    }

                    if(changeValue)
                        entity.CurrentValues[columnName] = DateTime.Now;
                }

                entity.State = _newState;
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Ruleta>().Property<Nullable<DateTime>>("DeletedAt");
            //modelBuilder.Entity<Ruleta>().HasQueryFilter(x => EF.Property<Nullable<DateTime>>(Ruletas, "DeletedAt") == null);
        }
    }
}
