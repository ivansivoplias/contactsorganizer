using System.Collections.Generic;

namespace Organizer.Common.Entities
{
    public class Contact : BaseEntity
    {
        public override string IdColumnName => "ContactId";

        public override string TableName => "dbo.Contacts";

        public string PrimaryPhone { get; set; }
        public int UserId { get; set; }
    }
}