using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Common.Interfaces;

public interface ITechnologyCapabilityRepository
{
    Task<IReadOnlyList<TechnologyCapability>> ListAsync(CancellationToken cancellationToken);
}
