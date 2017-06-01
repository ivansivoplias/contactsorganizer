using Organizer.Infrastructure;

namespace Organizer.DAL.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }
    }
}