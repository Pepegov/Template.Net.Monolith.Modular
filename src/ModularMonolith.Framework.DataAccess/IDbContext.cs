using System.Data.Common;

namespace ModularMonolith.Framework.DataAccess;

public interface IDbContext
{
    void UseTransaction(DbTransaction transaction);
    Task UseTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default);
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken token = default); 
}