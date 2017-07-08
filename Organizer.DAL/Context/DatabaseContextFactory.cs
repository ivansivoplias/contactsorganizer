using Organizer.Infrastructure.Database;

namespace Organizer.DAL.Context
{
    public class DatabaseContextFactory : IDatabaseContextFactory
    {
        private readonly string _connectionString;
        private IDbContext _dbContext;

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
            return _dbContext ?? (_dbContext = new DbContext(_connectionString));
        }

        /// <summary>
        /// Dispose existing context
        /// </summary>
        public void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}