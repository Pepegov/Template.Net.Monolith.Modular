using System.Data.Common;
using Microsoft.EntityFrameworkCore;
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

    public void UseTransaction(DbTransaction transaction)
        => Database.UseTransaction(transaction);

    public Task UseTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default)
        => Database.UseTransactionAsync(transaction, cancellationToken);
}