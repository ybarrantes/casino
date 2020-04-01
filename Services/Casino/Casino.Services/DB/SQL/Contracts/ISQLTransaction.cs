using System.Threading.Tasks;

namespace Casino.Services.DB.SQL.Contracts
{
    public interface ISQLTransaction
    {
        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

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
