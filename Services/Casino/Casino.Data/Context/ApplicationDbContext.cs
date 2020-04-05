using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using Casino.Services.DB.SQL.Contracts;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Data.Migrations.Configuration;

namespace Casino.Data.Context
{
    public class ApplicationDbContext : DbContext, ISqlTransaction
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        #region Datasets

        public DbSet<User> Users { get; set; }

        public DbSet<RouletteType> RouletteTypes { get; set; }
        public DbSet<RouletteState> RouletteStates { get; set; }
        public DbSet<Roulette> Roulettes { get; set; }

        public DbSet<RoundState> RoundStates { get; set; }
        public DbSet<Round> Rounds { get; set; }

        public DbSet<UserAccountType> UserAccountTypes { get; set; }
        public DbSet<UserAccountState> UserAccountStates { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        //public DbSet<Bet> Bets { get; set; }

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
            IEntityModelTimestamps.BeforeSave(ChangeTracker);
            IEntityModelSoftDeletes.BeforeSave(ChangeTracker);
        }

        #endregion


        // TODO: add filter to skip records marked as soft-deleted
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RouletteStateConfiguration());
            modelBuilder.ApplyConfiguration(new RouletteTypeConfiguration());

            modelBuilder.ApplyConfiguration(new RoundStateConfiguration());
            
            modelBuilder.ApplyConfiguration(new UserAccountStateConfiguration());
            modelBuilder.ApplyConfiguration(new UserAccountTypeConfiguration());
        }


        #region Implemented Members

        private IDbContextTransaction _transaction = null;
        
        private IDbContextTransaction Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                TransactionId = (Transaction == null) ? "" : Transaction.TransactionId.ToString();
            }
        }

        public bool HasTransaction => Transaction != null;
        public string TransactionId { get; internal set; }

        public async Task BeginTransactionAsync()
        {
            if (HasTransaction)
                throw new InvalidOperationException($"Transaction '{_transaction.TransactionId}' in process");

            Transaction = await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await ApplyActionTransactionAsync(ActionTransaction.Commit);
            await ApplyActionTransactionAsync(ActionTransaction.Dispose);
            Transaction = null;
        }

        public async Task RollbackTransactionAsync(bool throwException = true)
        {
            await ApplyActionTransactionAsync(ActionTransaction.Rollback);
            await ApplyActionTransactionAsync(ActionTransaction.Dispose);
            Transaction = null;

            if (throwException)
                throw new DbUpdateException("rollback transaction");
        }

        private async Task ApplyActionTransactionAsync(ActionTransaction action)
        {
            ThrowExceptionIfNotHasTransaction();

            try
            {
                if(action.Equals(ActionTransaction.Commit))
                    await Transaction.CommitAsync();
                else if(action.Equals(ActionTransaction.Rollback))
                    await Transaction.RollbackAsync();
                else if (action.Equals(ActionTransaction.Dispose))
                    await Transaction.DisposeAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        private void ThrowExceptionIfNotHasTransaction()
        {
            if(!HasTransaction)
                throw new NullReferenceException("No transacction in process");
        }

        #endregion
    }
}
