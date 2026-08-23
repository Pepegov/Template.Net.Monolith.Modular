using Template.Net.Monolith.Modular.Framework.Entities;

namespace ModularMonolith.Module.Template.Domain.Agreggates;

public class Template(string field) : Aggregate<Guid>
{
    public string Field { get; set; } = field;
}