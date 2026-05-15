using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Repositories;

public sealed class TechnologyCapabilityRepository(ApplicationDbContext dbContext) : ITechnologyCapabilityRepository
{
    public async Task<IReadOnlyList<TechnologyCapability>> ListAsync(CancellationToken cancellationToken)
    {
        return await dbContext.TechnologyCapabilities
            .AsNoTracking()
            .OrderBy(item => item.Id)
            .ToListAsync(cancellationToken);
    }
}
