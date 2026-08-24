using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace ModularMonolith.Framework.DataAccess.UnitOfWork;

public class ModularEfUnitOfWork(IServiceProvider serviceProvider) : IUnitOfWork 
{
    private IDbContextTransaction? _transaction;
    private readonly List<IContract> _contracts = [];
    
    public void UseTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
        _contracts.ForEach(x => x.DbContext.UseTransaction(_transaction));
    }

    public async Task UseTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        _transaction = transaction;
        foreach (var contract in _contracts)
        {
            await contract.DbContext.UseTransactionAsync(_transaction, cancellationToken: cancellationToken);
        }
    }

    public Task RollbackTransaction(CancellationToken ct = default)
        => _transaction is null ? Task.CompletedTask : _transaction.RollbackAsync(ct);
    

    public Task CommitTransaction(CancellationToken ct = default)
        => _transaction is null ? Task.CompletedTask : _transaction.CommitAsync(ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var contract in _contracts)
        {
            await contract.DbContext.SaveChangesAsync(ct);
        }
    }

    public TContract GetRequiredContract<TContract>() where TContract : class, IContract
    {
        var obj = serviceProvider.GetService(typeof(TContract));
        ArgumentNullException.ThrowIfNull(obj, nameof(TContract));
        var impl = (obj as TContract)!;

        if (_transaction is not null)
        {
            impl.DbContext.UseTransactionAsync(_transaction);
        }
        _contracts.Add(impl);
        
        return impl;
    }
}