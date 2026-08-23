using Microsoft.EntityFrameworkCore;
using ModularMonolith.Framework.DataAccess;

namespace ModularMonolith.Module.Template.DataAccess.Interface;

public interface ITemplateDbContext : IDbContext
{
    DbSet<Domain.Agreggates.Template> Templates { get; set; }
}