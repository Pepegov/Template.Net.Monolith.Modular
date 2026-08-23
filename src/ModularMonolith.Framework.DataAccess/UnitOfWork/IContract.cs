namespace ModularMonolith.Framework.DataAccess.UnitOfWork;

public interface IContract
{
    IDbContext DbContext { get; }
}