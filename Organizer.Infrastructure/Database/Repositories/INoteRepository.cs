using Organizer.Common.Entities;
using System;
using System.Collections.Generic;
using Organizer.Common.Enums;

namespace Organizer.Infrastructure.Database
{
    public interface INoteRepository : IRepository<Note>
    {
        Note GetNoteByCaption(int userId, string caption);

        IEnumerable<Note> GetUserNotes(int userId, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByCreationDate(int userId, DateTime date, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByLastChangeDate(int userId, DateTime lastChangeDate, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByNoteType(int userId, NoteType noteType, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByCurrentState(int userId, State state, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByPriority(int userId, Priority priority, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByCreationBetween(int userId, DateTime startLimit, DateTime endLimit, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByStartDate(int userId, DateTime startDate, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByEndDate(int userId, DateTime endDate, int? pageSize = null, int? page = null);

        IEnumerable<Note> FilterByCaptionLike(int userId, string caption, int? pageSize = null, int? page = null);
    }
}