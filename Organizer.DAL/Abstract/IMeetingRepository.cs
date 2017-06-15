using Organizer.Common.Entities;
using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;

namespace Organizer.DAL.Abstract
{
    public interface IMeetingRepository : IRepository<Meeting>
    {
        IEnumerable<Meeting> FilterByMeetingDate(int userId, DateTime meetingDate);

        IEnumerable<Meeting> FilterByMeetingName(int userId, string meetingName);
    }
}