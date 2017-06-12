using System.Collections.Generic;

namespace Organizer.DAL.Entities
{
    public class Contact : BaseEntity
    {
        public string PrimaryPhone { get; set; }
        public IEnumerable<string> SecondaryPhones { get; set; }
        public int UserId { get; set; }

        public override string IdColumnName => "ContactId";

        public override string TableName => "Contacts";
    }
}