using Organizer.Common.Enums;
using System;

namespace Organizer.Common.Entities
{
    public class Meeting : BaseEntity
    {
        public string MeetingName { get; set; }

        public string Description { get; set; }

        public MeetingType MeetingType { get; set; }

        public DateTime MeetingDate { get; set; }

        public DateTime NotificationDate { get; set; }

        public string MeetingPlace { get; set; }

        public TimeSpan NotificationTime { get; set; }

        public TimeSpan MeetingTime { get; set; }

        public bool SendNotifications { get; set; }

        public int UserId { get; set; }
    }
}