using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ModularMonolith.Module.Template.DataAccess.Interface;

namespace ModularMonolith.Module.Template.DataAccess.Sqlite;

public class TemplateDbContext : DbContext, ITemplateDbContext
{
    public const string Schema = "Template";

    public DbSet<Domain.Agreggates.Template> Templates { get; set; }

    public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
    }

    public void UseTransaction(IDbContextTransaction transaction)
        => Database.UseTransaction(transaction.GetDbTransaction());

    public Task UseTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
        => Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);

    public IDbContextTransaction BeginTransaction()
        => Database.BeginTransaction();

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => Database.BeginTransactionAsync(cancellationToken);
}