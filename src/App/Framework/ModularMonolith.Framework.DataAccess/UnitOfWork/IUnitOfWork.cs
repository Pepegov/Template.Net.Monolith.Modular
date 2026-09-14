using Microsoft.EntityFrameworkCore.Storage;

namespace ModularMonolith.Framework.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    void UseTransaction(IDbContextTransaction transaction);
    Task UseTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
    Task RollbackTransaction(CancellationToken ct = default);
    Task CommitTransaction(CancellationToken ct = default);
    
    Task SaveChangesAsync(CancellationToken ct = default);

    TContract GetRequiredContract<TContract>() where TContract : class, IContract;
}