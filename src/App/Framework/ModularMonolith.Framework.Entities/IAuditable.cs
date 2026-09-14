namespace Template.Net.Monolith.Modular.Framework.Entities;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
    string CreatedBy { get; set; }
    string? ModifiedBy { get; set; }
}