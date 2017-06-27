using Organizer.Common.Entities;
using System;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface IMeetingRepository : IRepository<Meeting>
    {
        IEnumerable<Meeting> GetUserMeetings(int userId, int? pageSize = null, int? page = null);

        Meeting FindByMeetingName(string meetingName);

        IEnumerable<Meeting> FilterByMeetingDate(int userId, DateTime meetingDate, int? pageSize = null,
            int? page = null);

        IEnumerable<Meeting> FilterByMeetingNameLike(int userId, string meetingName, int? pageSize = null,
            int? page = null);
    }
}