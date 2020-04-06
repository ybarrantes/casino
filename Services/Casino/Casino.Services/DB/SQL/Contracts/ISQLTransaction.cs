using System.Threading.Tasks;

namespace Casino.Services.DB.SQL.Contracts
{
    public interface ISqlTransaction
    {
        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync(bool throwException = true);

        bool HasTransaction { get; }

        string TransactionId { get; }
    }

    public enum ActionTransaction
    {
        Commit,
        Rollback,
        Dispose
    }
}
