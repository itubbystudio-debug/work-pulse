namespace WorkPulse.Domain.Entities;

public sealed class TechnologyCapability
{
    public int Id { get; init; }
    public required string Category { get; init; }
    public required string Technology { get; init; }
    public required string Role { get; init; }
}
