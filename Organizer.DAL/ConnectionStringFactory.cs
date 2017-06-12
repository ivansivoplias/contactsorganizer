using Organizer.Infrastructure;
using System.Configuration;

namespace Organizer.DAL
{
    public class ConnectionStringFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public ConnectionStringFactory(ConnectionStringSettings connectionString)
        {
            _connectionString = connectionString.ConnectionString;
        }

        public string ConnectionString => _connectionString;
    }
}