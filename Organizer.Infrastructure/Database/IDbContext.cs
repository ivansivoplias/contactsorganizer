using System;
using System.Data;

namespace Organizer.Infrastructure.Database
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Connection { get; }
    }
}