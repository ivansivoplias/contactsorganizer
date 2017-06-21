using Organizer.Common.Entities;
using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using Organizer.Common.Enums;

namespace Organizer.DAL.Abstract
{
    public interface INoteRepository : IRepository<Note>
    {
        IEnumerable<Note> FilterByCreationDate(int userId, DateTime date);

        IEnumerable<Note> FilterByLastChangeDate(int userId, DateTime lastChangeDate);

        IEnumerable<Note> FilterByNoteType(int userId, NoteType noteType);

        IEnumerable<Note> FilterByCurrentState(int userId, State state);

        IEnumerable<Note> FilterByPriority(int userId, Priority priority);

        IEnumerable<Note> FilterByCreationBetween(int userId, DateTime startLimit, DateTime endLimit);

        IEnumerable<Note> FilterByStartDate(int userId, DateTime startDate);

        IEnumerable<Note> FilterByEndDate(int userId, DateTime endDate);
    }
}