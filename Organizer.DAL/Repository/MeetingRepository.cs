using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Organizer.DAL.Repository
{
    public class MeetingRepository : RepositoryBase<Meeting>
    {
        public MeetingRepository(IDbContext context) : base(context, "Meetings")
        {
        }

        public override void Create(Meeting entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Meeting entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Meeting Get(int id)
        {
            throw new NotImplementedException();
        }

        public override Meeting Get(object key)
        {
            throw new NotImplementedException();
        }

        public override ICollection<Meeting> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Meeting Map(IDataRecord record)
        {
            var meeting = new Meeting();
            meeting.Id = int.Parse(record["MeetingId"] as string);
            meeting.Description = record["Description"] as string;
            meeting.MeetingDate = DateTime.Parse(record["MeetingDate"] as string);
            meeting.NotificationDate = DateTime.Parse(record["NotificationDate"] as string);
            meeting.SendNotification = bool.Parse(record["SendNotification"] as string);
            meeting.UserId = int.Parse(record["UserId"] as string);
            return meeting;
        }

        public override ICollection<Meeting> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(Meeting entity)
        {
            throw new NotImplementedException();
        }
    }
}