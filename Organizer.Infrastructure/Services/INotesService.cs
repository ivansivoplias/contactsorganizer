using System;
using System.Collections.Generic;
using Organizer.Common.Entities;
using Organizer.Common.Enums;

namespace Organizer.Infrastructure.Services
{
    public interface INotesService
    {
        void AddNote(Note note);

        void RemoveNote(Note note);

        void EditNote(Note note);

        ICollection<Note> GetNotes(User user);

        Note GetNote(int noteId);

        Note GetNoteByCaption(User id, string caption);

        ICollection<Note> GetNotesByCreationDate(User user, DateTime creationDate);

        ICollection<Note> GetNotesByLastChangeDate(User user, DateTime lastChangeDate);

        ICollection<Note> GetNotesByNoteType(User user, NoteType noteType);

        ICollection<Note> GetNotesByCurrentState(User user, State state);

        ICollection<Note> GetNotesByPriority(User user, Priority priority);

        ICollection<Note> GetNotesCreatedBetween(User user, DateTime start, DateTime end);

        ICollection<Note> GetNotesByStartDate(User user, DateTime startDate);

        ICollection<Note> GetNotesByEndDate(User user, DateTime endDate);
    }
}