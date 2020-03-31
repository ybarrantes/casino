using Microsoft.EntityFrameworkCore;
using Casino.API.Data.Entities;
using System;
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

        #region Datasets

        public DbSet<User> Users { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Roulette> Roulettes { get; set; }

        #endregion


        #region Save Changes

        public override int SaveChanges()
        {
            OnBeforeSave();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSave()
        {
            ITimestampsEntityModel.BeforeSave(ChangeTracker);
            ISoftDeletesEntityModel.BeforeSave(ChangeTracker);
        }

        #endregion


        // TODO: add filter to skip records marked as soft-deleted
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: ignore models on creating migrations, delete lines
            //modelBuilder.Ignore<User>();
            //modelBuilder.Ignore<Roulette>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
