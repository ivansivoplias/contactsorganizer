using Organizer.Common.Entities;
using System;
using System.Collections.Generic;
using Organizer.Common.Enums;

namespace Organizer.Infrastructure.Database
{
    public interface INoteRepository : IRepository<Note>
    {
        Note GetNoteByCaption(int userId, string caption);

        IEnumerable<Note> GetUserNotes(int userId);

        IEnumerable<Note> FilterByCreationDate(int userId, DateTime date);

        IEnumerable<Note> FilterByLastChangeDate(int userId, DateTime lastChangeDate);

        IEnumerable<Note> FilterByNoteType(int userId, NoteType noteType);

        IEnumerable<Note> FilterByCurrentState(int userId, State state);

        IEnumerable<Note> FilterByPriority(int userId, Priority priority);

        IEnumerable<Note> FilterByCreationBetween(int userId, DateTime startLimit, DateTime endLimit);

        IEnumerable<Note> FilterByStartDate(int userId, DateTime startDate);

        IEnumerable<Note> FilterByEndDate(int userId, DateTime endDate);

        IEnumerable<Note> FilterByCaptionLike(int userId, string caption);
    }
}