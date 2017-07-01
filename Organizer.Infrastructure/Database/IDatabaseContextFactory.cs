namespace Organizer.Infrastructure.Database
{
    public interface IDatabaseContextFactory
    {
        IDbContext MakeContext();
    }
}