namespace Organizer.Infrastructure
{
    public interface IConnectionFactory
    {
        string ConnectionString { get; }
    }
}