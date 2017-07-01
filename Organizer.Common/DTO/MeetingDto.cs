using System;

namespace Organizer.Common.DTO
{
    public class MeetingDto : BaseDto
    {
        public string MeetingName { get; set; }

        public string Description { get; set; }

        public DateTime MeetingDate { get; set; }

        public DateTime NotificationDate { get; set; }

        public bool SendNotifications { get; set; }

        public int UserId { get; set; }
    }
}