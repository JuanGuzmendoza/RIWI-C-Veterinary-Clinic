namespace VeterinaryClinic.interfaces;

/// <summary>
/// Interface that defines an entity with a unique identifier in the HealthClinic system.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    Guid Id { get; }
}