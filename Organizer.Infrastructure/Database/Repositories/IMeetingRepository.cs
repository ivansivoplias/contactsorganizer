using Organizer.Common.Entities;
using System;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface IMeetingRepository : IRepository<Meeting>
    {
        IEnumerable<Meeting> GetUserMeetings(int userId);

        Meeting FindByMeetingName(string meetingName);

        IEnumerable<Meeting> FilterByMeetingDate(int userId, DateTime meetingDate);

        IEnumerable<Meeting> FilterByMeetingNameLike(int userId, string meetingName);
    }
}