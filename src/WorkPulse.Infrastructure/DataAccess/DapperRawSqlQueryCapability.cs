using Dapper;
using WorkPulse.Application.Common.Interfaces;

namespace WorkPulse.Infrastructure.DataAccess;

public sealed class DapperRawSqlQueryCapability(SqlConnectionFactory connectionFactory) : IRawSqlQueryCapability
{
    public string PrimaryDatabase => "SQL Server";

    public string PrimaryOrm => "EF Core";

    public string RawSqlTechnology => "Dapper";

    public bool IsConfigured
    {
        get
        {
            using var connection = connectionFactory.Create();
            var _ = new CommandDefinition("SELECT 1");
            return connection.ConnectionString?.Contains("Server=", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
