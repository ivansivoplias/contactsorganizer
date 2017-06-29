using Organizer.Common.Entities;
using Organizer.Common.Enums;
using System;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface INoteRepository : IRepository<Note>
    {
        Note GetNoteByCaption(int userId, string caption);

        IEnumerable<Note> GetUserNotes(int userId, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByCreationDate(int userId, DateTime date, NoteType noteType, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByLastChangeDate(int userId, DateTime lastChangeDate, NoteType noteType, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByNoteType(int userId, NoteType noteType, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByCurrentState(int userId, State state, NoteType noteType, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByPriority(int userId, Priority priority, NoteType noteType, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByCreationBetween(int userId, DateTime startLimit, DateTime endLimit, NoteType noteType, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByStartDate(int userId, DateTime startDate, NoteType noteType, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByEndDate(int userId, DateTime endDate, NoteType noteType, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByCaptionLike(int userId, string caption, NoteType noteType, int? pageSize = null, int? page = null);
    }
}