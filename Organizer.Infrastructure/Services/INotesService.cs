using System;
using System.Collections.Generic;
using Organizer.Common.Enums;
using Organizer.Common.DTO;

namespace Organizer.Infrastructure.Services
{
    public interface INotesService
    {
        void AddNote(NoteDto note);

        void RemoveNote(NoteDto note);

        void EditNote(NoteDto note);

        ICollection<NoteDto> GetNotes(UserDto user);

        NoteDto GetNote(int noteId);

        NoteDto GetNoteByCaption(UserDto user, string caption);

        ICollection<NoteDto> GetNotesByCreationDate(UserDto user, DateTime creationDate);

        ICollection<NoteDto> GetNotesByLastChangeDate(UserDto user, DateTime lastChangeDate);

        ICollection<NoteDto> GetNotesByNoteType(UserDto user, NoteType noteType);

        ICollection<NoteDto> GetNotesByCurrentState(UserDto user, State state);

        ICollection<NoteDto> GetNotesByPriority(UserDto user, Priority priority);

        ICollection<NoteDto> GetNotesCreatedBetween(UserDto user, DateTime start, DateTime end);

        ICollection<NoteDto> GetNotesByStartDate(UserDto user, DateTime startDate);

        ICollection<NoteDto> GetNotesByEndDate(UserDto user, DateTime endDate);
    }
}