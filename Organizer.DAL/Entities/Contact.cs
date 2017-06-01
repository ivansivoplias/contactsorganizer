using Organizer.Infrastructure;

namespace Organizer.DAL.Entities
{
    public class Contact : IEntity
    {
        public int Id { get; set; }

        public string Phone { get; set; }

        public int UserId { get; set; }
    }
}