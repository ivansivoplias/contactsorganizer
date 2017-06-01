using Organizer.Infrastructure;

namespace Organizer.DAL.Entities
{
    public class PersonalInfo : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string MiddleName { get; set; }

        public string Nickname { get; set; }

        public string Skype { get; set; }

        public string Email { get; set; }
    }
}