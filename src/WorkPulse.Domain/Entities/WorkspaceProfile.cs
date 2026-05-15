namespace WorkPulse.Domain.Entities;

public class WorkspaceProfile
{
    private WorkspaceProfile()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public static WorkspaceProfile Create(string name, DateTime createdAtUtc)
    {
        return new WorkspaceProfile
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            CreatedAtUtc = createdAtUtc
        };
    }
}
