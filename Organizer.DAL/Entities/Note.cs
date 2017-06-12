using Organizer.Common;
using System;

namespace Organizer.DAL.Entities
{
    public class Note : BaseEntity
    {
        public string NoteText { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public NoteType NoteType { get; set; }

        public State? State { get; set; }
        public Priority? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int UserId { get; set; }

        public override string IdColumnName => "NoteId";

        public override string TableName => "Notes";
    }
}