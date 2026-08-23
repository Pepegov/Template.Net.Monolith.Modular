using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ModularMonolith.Module.Builder.DataAccess.Sqlite;

public class BuilderDbContextFactory : IDesignTimeDbContextFactory<BuilderDbContext>
{
    public BuilderDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<BuilderDbContext>()
            .UseSqlite("Data Source=modular.db");
        return new BuilderDbContext(options.Options);
    }
}
