using System.Data;

namespace Organizer.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection MakeConnection();
    }
}