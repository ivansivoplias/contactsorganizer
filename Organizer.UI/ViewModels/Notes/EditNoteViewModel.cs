using Autofac;
using Organizer.Common.DTO;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class EditNoteViewModel : ViewModelBase
    {
        private Command _saveCommand;
        private Command _cancelCommand;
        private NoteDto _note;
        private INoteService _noteService;

        public event EventHandler SaveMessage = delegate { };

        public event EventHandler CancelMessage = delegate { };

        public ICommand SaveCommand => _saveCommand;

        public ICommand CancelCommand => _cancelCommand;

        public string Caption
        {
            get { return _note.Caption; }
            set
            {
                _note.Caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public string NoteText
        {
            get { return _note.NoteText; }
            set
            {
                _note.NoteText = value;
                OnPropertyChanged(nameof(NoteText));
            }
        }

        public EditNoteViewModel(NoteDto note)
        {
            _noteService = App.Containter.Resolve<INoteService>();

            _note = note;

            _saveCommand = Command.CreateCommand("Save note", "SaveCommand", GetType(), Save);
            _cancelCommand = Command.CreateCommand("Cancel", "CancelCommand", GetType(), Cancel);
        }

        private void Save()
        {
            try
            {
                _note.LastChangeDate = DateTime.Now;
                _noteService.EditNote(_note);
                SaveMessage.Invoke(null, EventArgs.Empty);
            }
            catch
            {
                MessageBox.Show("Invalid data provided. Note cannot be saved.", "Error");
            }
        }

        private void Cancel()
        {
            CancelMessage.Invoke(null, EventArgs.Empty);
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _saveCommand);
            Command.RegisterCommandBinding(window, _cancelCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _saveCommand);
            Command.UnregisterCommandBinding(window, _cancelCommand);
        }
    }
}