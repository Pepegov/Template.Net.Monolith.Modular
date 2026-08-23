using System.Data.Common;

namespace ModularMonolith.Framework.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    void UseTransaction(DbTransaction transaction);
    Task UseTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default);
    Task RollbackTransaction(CancellationToken ct = default);
    Task CommitTransaction(CancellationToken ct = default);
    
    Task SaveChangesAsync(CancellationToken ct = default);

    TContract GetRequiredContract<TContract>() where TContract : class, IContract;
}