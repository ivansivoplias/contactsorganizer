using Organizer.Common.Entities;
using System;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Services
{
    public interface IMeetingService
    {
        void AddMeeting(Meeting meeting);

        void RemoveMeeting(Meeting meeting);

        void EditMeeting(Meeting meeting);

        Meeting GetMeeting(int meetingId);

        Meeting GetMeetingByName(int userId, string meetingName);

        ICollection<Meeting> GetUserMeetings(User user, int pageSize, int page);

        ICollection<Meeting> FilterByMeetingDate(User user, DateTime meetingDate, int pageSize, int page);

        ICollection<Meeting> FilterByMeetingName(User user, string meetingName, int pageSize, int page);
    }
}