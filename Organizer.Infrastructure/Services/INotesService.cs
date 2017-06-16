using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.BL.Abstract
{
    public interface INotesService
    {
        void AddNote(Note note);

        void RemoveNote(Note note);

        void EditNote(Note note);

        ICollection<Note> GetNotes(User user);

        Note GetNote(int noteId);
    }
}