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

        int GetMeetingsCount(User user);

        ICollection<Meeting> FilterByMeetingDate(User user, DateTime meetingDate, int pageSize, int page);

        int GetFilterByMeetingDateCount(User user, DateTime meetingDate);

        ICollection<Meeting> FilterByMeetingName(User user, string meetingName, int pageSize, int page);

        int GetFilterByMeetingNameCount(User user, string meetingName);
    }
}