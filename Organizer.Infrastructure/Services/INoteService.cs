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

        int GetNotesCount(User user);

        Note GetNote(int noteId);

        Note GetNoteByCaption(User user, NoteType noteType, string caption);

        ICollection<Note> GetNotesByCaptionLike(User user, string caption, NoteType noteType, int pageSize, int page);

        int GetNotesByCaptionLikeCount(User user, string caption, NoteType noteType);

        ICollection<Note> GetNotesByCreationDate(User user, DateTime creationDate, NoteType noteType, int pageSize, int page);

        int GetNotesByCreationDateCount(User user, DateTime creationDate, NoteType noteType);

        ICollection<Note> GetNotesByLastChangeDate(User user, DateTime lastChangeDate, NoteType noteType, int pageSize, int page);

        int GetNotesByLastChangeDateCount(User user, DateTime lastChangeDate, NoteType noteType);

        ICollection<Note> GetNotesByNoteType(User user, NoteType noteType, int pageSize, int page);

        int GetNotesByNoteTypeCount(User user, NoteType noteType);

        ICollection<Note> GetNotesByCurrentState(User user, State state, NoteType noteType, int pageSize, int page);

        int GetNotesByCurrentStateCount(User user, State state, NoteType noteType);

        ICollection<Note> GetNotesByPriority(User user, Priority priority, NoteType noteType, int pageSize, int page);

        int GetNotesByPriorityCount(User user, Priority priority, NoteType noteType);

        ICollection<Note> GetNotesCreatedBetween(User user, DateTime start, DateTime end, NoteType noteType, int pageSize, int page);

        int GetNotesCreatedBetweenCount(User user, DateTime start, DateTime end, NoteType noteType);

        ICollection<Note> GetNotesByStartDate(User user, DateTime startDate, NoteType noteType, int pageSize, int page);

        int GetNotesByStartDateCount(User user, DateTime startDate, NoteType noteType);

        ICollection<Note> GetNotesByEndDate(User user, DateTime endDate, NoteType noteType, int pageSize, int page);

        int GetNotesByEndDateCount(User user, DateTime endDate, NoteType noteType);
    }
}