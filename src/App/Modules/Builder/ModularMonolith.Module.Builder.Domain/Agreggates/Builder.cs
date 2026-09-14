using Template.Net.Monolith.Modular.Framework.Entities;

namespace ModularMonolith.Module.Builder.Domain.Agreggates;

public class Builder(string field) : Aggregate<Guid>
{
    public string Field { get; set; } = field;

    public Guid TemplateId { get; set; }
}
