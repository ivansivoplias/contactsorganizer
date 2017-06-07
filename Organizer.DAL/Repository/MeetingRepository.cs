using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace Organizer.DAL.Repository
{
    public class MeetingRepository : RepositoryBase<Meeting>
    {
        private const string TableName = "Meetings";
        private DataTable _meetingsTable;

        public MeetingRepository(IDbContext context) : base(context, TableName)
        {
            _meetingsTable = _dataSet.Tables[TableName];
        }

        public override void Create(Meeting entity)
        {
            DataRow meetingRow = _meetingsTable.NewRow();
            meetingRow["Description"] = entity.Description;
            meetingRow["MeetingDate"] = entity.MeetingDate;
            meetingRow["NotificationDate"] = entity.NotificationDate;
            meetingRow["SendNotification"] = entity.SendNotification;
            meetingRow["UserId"] = entity.UserId;
            _meetingsTable.Rows.Add(meetingRow);
        }

        public override void Delete(Meeting entity)
        {
            Delete(entity.Id);
        }

        public override void Delete(int id)
        {
            try
            {
                var datarow = _meetingsTable.Rows.Find(id);
                _meetingsTable.Rows.Remove(datarow);
            }
            catch (MissingPrimaryKeyException) { }
        }

        public override Meeting Get(int id)
        {
            Meeting result = null;
            try
            {
                var datarow = _meetingsTable.Rows.Find(id);
                result = Map(datarow);
            }
            catch (MissingPrimaryKeyException) { }

            return result;
        }

        public override Meeting Get(object key)
        {
            throw new NotImplementedException();
        }

        public override ICollection<Meeting> GetAll()
        {
            var result = new List<Meeting>();
            var dataRows = _meetingsTable.Select();
            if (dataRows != null && dataRows.Length > 0)
            {
                foreach (var row in dataRows)
                {
                    result.Add(Map(row));
                }
            }
            return result;
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

        public override Meeting Map(DataRow row)
        {
            var meeting = new Meeting();
            meeting.Id = int.Parse(row["MeetingId"] as string);
            meeting.Description = row["Description"] as string;
            meeting.MeetingDate = DateTime.Parse(row["MeetingDate"] as string);
            meeting.NotificationDate = DateTime.Parse(row["NotificationDate"] as string);
            meeting.SendNotification = bool.Parse(row["SendNotification"] as string);
            meeting.UserId = int.Parse(row["UserId"] as string);
            return meeting;
        }

        public override ICollection<Meeting> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(Meeting entity)
        {
            try
            {
                var row = _meetingsTable.Rows.Find(entity.Id);
                row.BeginEdit();
                row["Description"] = entity.Description;
                row["MeetingDate"] = entity.MeetingDate;
                row["NotificationDate"] = entity.NotificationDate;
                row["SendNotification"] = entity.SendNotification;
                row["UserId"] = entity.UserId;
                row.EndEdit();
            }
            catch (MissingPrimaryKeyException) { }
        }
    }
}