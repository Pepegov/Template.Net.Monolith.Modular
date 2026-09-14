using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ModularMonolith.Module.Builder.DataAccess.Interface;

namespace ModularMonolith.Module.Builder.DataAccess.Sqlite;

public class BuilderDbContext(DbContextOptions<BuilderDbContext> options) : DbContext(options), IBuilderDbContext
{
    public const string Schema = "Builder";

    public DbSet<Domain.Agreggates.Builder> Builders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<Domain.Agreggates.Builder>(builder =>
        {
            builder.Property(b => b.TemplateId).IsRequired();
        });
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
