using Organizer.Common.DTO;
using System;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Services
{
    public interface IMeetingService
    {
        void AddMeeting(MeetingDto meeting);

        void RemoveMeeting(MeetingDto meeting);

        void EditMeeting(MeetingDto meeting);

        MeetingDto GetMeeting(int meetingId);

        MeetingDto GetMeetingByName(int userId, string meetingName);

        ICollection<MeetingDto> GetUserMeetings(UserDto user, int pageSize, int page);

        ICollection<MeetingDto> FilterByMeetingDate(UserDto user, DateTime meetingDate, int pageSize, int page);

        ICollection<MeetingDto> FilterByMeetingName(UserDto user, string meetingName, int pageSize, int page);
    }
}