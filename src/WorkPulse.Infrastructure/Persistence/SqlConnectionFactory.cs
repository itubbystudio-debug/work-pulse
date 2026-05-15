using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WorkPulse.Application.Common.Interfaces;

namespace WorkPulse.Infrastructure.Persistence;

public sealed class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    public async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
