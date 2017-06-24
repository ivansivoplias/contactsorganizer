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

        MeetingDto GetMeetingByName(string meetingName);

        ICollection<MeetingDto> GetUserMeetings(UserDto user);

        ICollection<MeetingDto> FilterByMeetingDate(UserDto user, DateTime meetingDate);

        ICollection<MeetingDto> FilterByMeetingName(UserDto user, string meetingName);
    }
}