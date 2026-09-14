using Microsoft.EntityFrameworkCore;
using ModularMonolith.Framework.DataAccess;

namespace ModularMonolith.Module.Builder.DataAccess.Interface;

public interface IBuilderDbContext : IDbContext
{
    DbSet<Domain.Agreggates.Builder> Builders { get; set; }
}
