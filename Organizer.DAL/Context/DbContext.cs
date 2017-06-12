using Organizer.Infrastructure.Database;
using System.Data;
using System.Data.SqlClient;

namespace Organizer.DAL.Context
{
    public class DbContext : IDbContext
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        /// <summary>
        /// Get connection string inside constructor.
        /// So when the class will be initialized then connection string will be automatically get from web.config
        /// </summary>
        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connectionString);
                }
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        /// <summary>
        /// Dispose Connection
        /// </summary>
        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}