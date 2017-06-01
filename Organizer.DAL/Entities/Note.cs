using Organizer.Common;
using Organizer.Infrastructure;
using System;

namespace Organizer.DAL.Entities
{
    public class Note : IEntity
    {
        public int Id { get; set; }

        public string NoteText { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastChangeDate { get; set; }

        public NoteType NoteType { get; set; }

        public State? State { get; set; }

        public Priority? Priority { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int UserId { get; set; }
    }
}