using System;
using System.Data.SqlClient;

namespace Organizer.Infrastructure.Database
{
    public interface IDbContext : IDisposable
    {
        SqlConnection Connection { get; }
    }
}