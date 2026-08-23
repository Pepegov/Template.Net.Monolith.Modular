namespace Template.Net.Monolith.Modular.Framework.Entities;

public abstract class Entity<TType> where TType : struct
{
    public TType Id { get; set; }
}