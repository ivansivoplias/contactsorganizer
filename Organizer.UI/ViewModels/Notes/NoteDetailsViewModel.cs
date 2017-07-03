using Organizer.Common.DTO;
using Organizer.Common.Entities;
using Organizer.UI.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class NoteDetailsViewModel : ViewModelBase
    {
        private Command _backCommand;
        private Note _note;

        public event EventHandler BackMessage = delegate { };

        public ICommand BackCommand => _backCommand;

        public string Caption => _note.Caption;

        public string NoteText => _note.NoteText;

        public DateTime CreationDate => _note.CreationDate;

        public DateTime LastChangeDate => _note.LastChangeDate;

        public NoteDetailsViewModel(Note note)
        {
            _note = note;

            _backCommand = Command.CreateCommand("Back to notes list", "BackCommand", GetType(), Back);
        }

        private void Back()
        {
            BackMessage.Invoke(null, EventArgs.Empty);
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _backCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _backCommand);
        }
    }
}