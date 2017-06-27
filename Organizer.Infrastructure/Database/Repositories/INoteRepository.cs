using Organizer.Common.Entities;
using System;
using System.Collections.Generic;
using Organizer.Common.Enums;

namespace Organizer.Infrastructure.Database
{
    public interface INoteRepository : IRepository<Note>
    {
        Note GetNoteByCaption(int userId, string caption);

        IEnumerable<Note> GetUserNotes(int userId, int pageSize = 10, int page = 1);

        IEnumerable<Note> FilterByCreationDate(int userId, DateTime date, int pageSize = 10, int page = 1);

        IEnumerable<Note> FilterByLastChangeDate(int userId, DateTime lastChangeDate, int pageSize = 10, int page = 1);

        IEnumerable<Note> FilterByNoteType(int userId, NoteType noteType, int pageSize = 10, int page = 1);

        IEnumerable<Note> FilterByCurrentState(int userId, State state, int pageSize = 10, int page = 1);

        IEnumerable<Note> FilterByPriority(int userId, Priority priority, int pageSize = 10, int page = 1);

        IEnumerable<Note> FilterByCreationBetween(int userId, DateTime startLimit, DateTime endLimit, int pageSize = 10, int page = 1);

        IEnumerable<Note> FilterByStartDate(int userId, DateTime startDate, int pageSize = 10, int page = 1);

        IEnumerable<Note> FilterByEndDate(int userId, DateTime endDate, int pageSize = 10, int page = 1);

        IEnumerable<Note> FilterByCaptionLike(int userId, string caption, int pageSize = 10, int page = 1);
    }
}