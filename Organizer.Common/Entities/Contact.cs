using System.Collections.Generic;

namespace Organizer.Common.Entities
{
    public class Contact : BaseEntity
    {
        public string PrimaryPhone { get; set; }
        public int UserId { get; set; }

        public override string IdColumnName => "ContactId";

        public override string TableName => "dbo.Contacts";
    }
}