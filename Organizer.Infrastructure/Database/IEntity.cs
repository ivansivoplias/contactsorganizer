namespace Organizer.Infrastructure.Database
{
    public interface IEntity
    {
        int Id { get; set; }

        string IdColumnName { get; }

        string TableName { get; }
    }
}