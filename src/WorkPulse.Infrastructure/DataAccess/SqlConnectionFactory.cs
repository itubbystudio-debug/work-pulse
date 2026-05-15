using System.Data;
using Microsoft.Data.SqlClient;

namespace WorkPulse.Infrastructure.DataAccess;

public sealed class SqlConnectionFactory(SqlServerOptions options)
{
    public IDbConnection Create()
    {
        return new SqlConnection(options.DefaultConnection);
    }
}
