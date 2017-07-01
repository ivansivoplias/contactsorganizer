using Organizer.Infrastructure.Database;

namespace Organizer.DAL.Context
{
    public class DatabaseContextFactory : IDatabaseContextFactory
    {
        private readonly string _connectionString;
        private IDbContext dataContext;

        public DatabaseContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// If data context remain null then initialize new context
        /// </summary>
        /// <returns>dataContext</returns>
        public IDbContext MakeContext()
        {
            return dataContext ?? (dataContext = new DbContext(_connectionString));
        }

        /// <summary>
        /// Dispose existing context
        /// </summary>
        public void Dispose()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}