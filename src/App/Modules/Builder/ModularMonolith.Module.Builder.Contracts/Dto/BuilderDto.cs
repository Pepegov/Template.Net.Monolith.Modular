namespace ModularMonolith.Module.Builder.Contracts.Dto;

public class BuilderDto
{
    public required string Field { get; set; }

    public required Guid TemplateId { get; set; }
}
