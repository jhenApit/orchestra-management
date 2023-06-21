using Microsoft.Data.SqlClient;
using System.Data;

namespace OrchestraAPI.Context
{
    /// <summary>
    /// A context wrapper class for our database.
    /// </summary>
    public class DapperContext
    {
        /// <summary>
        /// This service reads key-value pairs from `appsettings`.
        /// </summary>
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlServer");
        }

        /// <summary>
        /// Creates a database connection using the connection string we have provided in appsettings.
        /// </summary>
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
