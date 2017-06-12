using Organizer.Infrastructure.Database;

namespace Organizer.Common.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }

        public abstract string IdColumnName { get; }

        public abstract string TableName { get; }
    }
}