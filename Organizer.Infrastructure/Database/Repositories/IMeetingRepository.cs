using Organizer.Common.Entities;
using System;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface IMeetingRepository : IRepository<Meeting>
    {
        IEnumerable<Meeting> GetUserMeetings(int userId, int pageSize = 10, int page = 1);

        Meeting FindByMeetingName(string meetingName);

        IEnumerable<Meeting> FilterByMeetingDate(int userId, DateTime meetingDate, int pageSize = 10, int page = 1);

        IEnumerable<Meeting> FilterByMeetingNameLike(int userId, string meetingName, int pageSize = 10, int page = 1);
    }
}