namespace Organizer.Common.Entities
{
    public class PersonalInfo : BaseEntity
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string MiddleName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
    }
}