using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class CompanyHoliday : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private CompanyHoliday()
    {
        Name = string.Empty;
    }

    public CompanyHoliday(DateOnly date, string name)
    {
        Validate(date, name);

        Date = date;
        Name = name.Trim();
    }

    public DateOnly Date { get; private set; }

    public string Name { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public void Update(DateOnly date, string name)
    {
        Validate(date, name);

        Date = date;
        Name = name.Trim();
    }

    private static void Validate(DateOnly date, string name)
    {
        if (date == default)
        {
            throw new BusinessRuleViolationException("CompanyHoliday.DateRequired", "Holiday date is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("CompanyHoliday.NameRequired", "Holiday name is required.");
        }
    }
}
