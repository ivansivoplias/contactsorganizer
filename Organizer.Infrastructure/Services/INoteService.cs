using System;
using System.Collections.Generic;
using Organizer.Common.Enums;
using Organizer.Common.DTO;

namespace Organizer.Infrastructure.Services
{
    public interface INoteService
    {
        void AddNote(NoteDto note);

        void RemoveNote(NoteDto note);

        void EditNote(NoteDto note);

        ICollection<NoteDto> GetNotes(UserDto user, int pageSize, int page);

        NoteDto GetNote(int noteId);

        NoteDto GetNoteByCaption(UserDto user, string caption);

        ICollection<NoteDto> GetNotesByCreationDate(UserDto user, DateTime creationDate, int pageSize, int page);

        ICollection<NoteDto> GetNotesByLastChangeDate(UserDto user, DateTime lastChangeDate, int pageSize, int page);

        ICollection<NoteDto> GetNotesByNoteType(UserDto user, NoteType noteType, int pageSize, int page);

        ICollection<NoteDto> GetNotesByCurrentState(UserDto user, State state, int pageSize, int page);

        ICollection<NoteDto> GetNotesByPriority(UserDto user, Priority priority, int pageSize, int page);

        ICollection<NoteDto> GetNotesCreatedBetween(UserDto user, DateTime start, DateTime end, int pageSize, int page);

        ICollection<NoteDto> GetNotesByStartDate(UserDto user, DateTime startDate, int pageSize, int page);

        ICollection<NoteDto> GetNotesByEndDate(UserDto user, DateTime endDate, int pageSize, int page);
    }
}