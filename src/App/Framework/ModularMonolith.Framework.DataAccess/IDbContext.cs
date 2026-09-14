using Microsoft.EntityFrameworkCore.Storage;

namespace ModularMonolith.Framework.DataAccess;

public interface IDbContext
{
    void UseTransaction(IDbContextTransaction transaction);
    Task UseTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken token = default); 
}