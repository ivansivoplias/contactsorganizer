using Organizer.Infrastructure;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Organizer.DAL
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _provider;
        private readonly ConnectionStringSettings _connectionString;

        public ConnectionFactory(ConnectionStringSettings connectionString)
        {
            _connectionString = connectionString;
            _provider = DbProviderFactories.GetFactory(_connectionString.ProviderName);
        }

        public IDbConnection MakeConnection()
        {
            var connection = _provider.CreateConnection();
            if (connection == null)
                throw new ConfigurationErrorsException($"Failed to create a connection using the connection string named '{_connectionString.Name}' in app/web.config.");

            connection.ConnectionString = _connectionString.ConnectionString;
            connection.Open();
            return connection;
        }
    }
}