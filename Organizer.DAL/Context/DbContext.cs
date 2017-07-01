using Organizer.Infrastructure.Database;
using System.Data;
using System.Data.SqlClient;

namespace Organizer.DAL.Context
{
    public class DbContext : IDbContext
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

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

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}