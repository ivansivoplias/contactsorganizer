using System;

namespace Organizer.DAL.Entities
{
    public class Meeting : BaseEntity
    {
        public string Description { get; set; }
        public DateTime MeetingDate { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool SendNotifications { get; set; }
        public int UserId { get; set; }

        public override string IdColumnName => "MeetingId";

        public override string TableName => "Meetings";
    }
}