using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using Microsoft.Extensions.Logging;
using Casino.Services.DB.SQL.Contracts;
using Casino.Services.DB.SQL.Contracts.Model;

namespace Casino.Data.Context
{
    public class ApplicationDbContext : DbContext, ISQLTransaction
    {
        private readonly ILogger<ApplicationDbContext> _logger;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
            :base(options)
        {
            _logger = logger;
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
            IEntityModelTimestamps.BeforeSave(ChangeTracker);
            IEntityModelSoftDeletes.BeforeSave(ChangeTracker);
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

        public async Task RollbackTransactionAsync()
        {
            await ApplyActionTransactionAsync(ActionTransaction.Rollback);
            await ApplyActionTransactionAsync(ActionTransaction.Dispose);
            Transaction = null;
        }

        private async Task ApplyActionTransactionAsync(ActionTransaction action)
        {
            ThrowExceptionIfNotHasTransaction();

            try
            {
                _logger.LogInformation($"Trying to {action.ToString()} transaction [{TransactionId}]");

                if(action.Equals(ActionTransaction.Commit))
                    await Transaction.CommitAsync();
                else if(action.Equals(ActionTransaction.Rollback))
                    await Transaction.RollbackAsync();
                else if (action.Equals(ActionTransaction.Dispose))
                    await Transaction.DisposeAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{action.ToString()} failed, transaction id: {TransactionId}");
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
