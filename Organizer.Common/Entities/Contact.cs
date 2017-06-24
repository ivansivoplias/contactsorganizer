using System.Collections.Generic;

namespace Organizer.Common.Entities
{
    public class Contact : BaseEntity
    {
        public string PrimaryPhone { get; set; }
        public int UserId { get; set; }
    }
}