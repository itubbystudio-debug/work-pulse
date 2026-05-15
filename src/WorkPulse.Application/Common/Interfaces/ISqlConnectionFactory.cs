using System.Data;

namespace WorkPulse.Application.Common.Interfaces;

public interface ISqlConnectionFactory
{
    Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken);
}
