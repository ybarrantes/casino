using Casino.Services.DB.SQL.Contracts;
using Casino.Services.DB.SQL.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Casino.Services.DB.SQL.Context
{
    public class ApplicationDbContextBase : DbContext, ISqlTransaction
    {
        public ApplicationDbContextBase(DbContextOptions options)
            : base(options)
        {
        }

        public async Task<T> FindGenericElementByIdAsync<T>(long id) where T : class
        {
            return await this.Set<T>()
                .FirstOrDefaultAsync(x => (bool)(((IEntityModelBase)x).Id == id));
        }

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

        #region ISqlTransaction members

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
                if (action.Equals(ActionTransaction.Commit))
                    await Transaction.CommitAsync();
                else if (action.Equals(ActionTransaction.Rollback))
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
            if (!HasTransaction)
                throw new NullReferenceException("No transacction in process");
        }

        #endregion
    }
}
