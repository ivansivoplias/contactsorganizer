using Organizer.Common.DTO;
using Organizer.Common.Enums;
using System;
using System.Collections.Generic;

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

        ICollection<NoteDto> GetNotesByCaptionLike(UserDto user, string caption, NoteType noteType, int pageSize, int page);

        ICollection<NoteDto> GetNotesByCreationDate(UserDto user, DateTime creationDate, NoteType noteType, int pageSize, int page);

        ICollection<NoteDto> GetNotesByLastChangeDate(UserDto user, DateTime lastChangeDate, NoteType noteType, int pageSize, int page);

        ICollection<NoteDto> GetNotesByNoteType(UserDto user, NoteType noteType, int pageSize, int page);

        ICollection<NoteDto> GetNotesByCurrentState(UserDto user, State state, NoteType noteType, int pageSize, int page);

        ICollection<NoteDto> GetNotesByPriority(UserDto user, Priority priority, NoteType noteType, int pageSize, int page);

        ICollection<NoteDto> GetNotesCreatedBetween(UserDto user, DateTime start, DateTime end, NoteType noteType, int pageSize, int page);

        ICollection<NoteDto> GetNotesByStartDate(UserDto user, DateTime startDate, NoteType noteType, int pageSize, int page);

        ICollection<NoteDto> GetNotesByEndDate(UserDto user, DateTime endDate, NoteType noteType, int pageSize, int page);
    }
}