using Organizer.Common.Entities;
using Organizer.Common.Enums;
using System;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Services
{
    public interface INoteService
    {
        void AddNote(Note note);

        void RemoveNote(Note note);

        void EditNote(Note note);

        ICollection<Note> GetNotes(User user, int pageSize, int page);

        Note GetNote(int noteId);

        Note GetNoteByCaption(User user, NoteType noteType, string caption);

        ICollection<Note> GetNotesByCaptionLike(User user, string caption, NoteType noteType, int pageSize, int page);

        ICollection<Note> GetNotesByCreationDate(User user, DateTime creationDate, NoteType noteType, int pageSize, int page);

        ICollection<Note> GetNotesByLastChangeDate(User user, DateTime lastChangeDate, NoteType noteType, int pageSize, int page);

        ICollection<Note> GetNotesByNoteType(User user, NoteType noteType, int pageSize, int page);

        ICollection<Note> GetNotesByCurrentState(User user, State state, NoteType noteType, int pageSize, int page);

        ICollection<Note> GetNotesByPriority(User user, Priority priority, NoteType noteType, int pageSize, int page);

        ICollection<Note> GetNotesCreatedBetween(User user, DateTime start, DateTime end, NoteType noteType, int pageSize, int page);

        ICollection<Note> GetNotesByStartDate(User user, DateTime startDate, NoteType noteType, int pageSize, int page);

        ICollection<Note> GetNotesByEndDate(User user, DateTime endDate, NoteType noteType, int pageSize, int page);
    }
}