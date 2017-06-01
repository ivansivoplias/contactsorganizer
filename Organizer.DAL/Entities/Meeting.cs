using Organizer.Infrastructure;
using System;

namespace Organizer.DAL.Entities
{
    public class Meeting : IEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime MeetingDate { get; set; }

        public DateTime NotificationDate { get; set; }

        public bool SendNotification { get; set; }

        public int UserId { get; set; }
    }
}