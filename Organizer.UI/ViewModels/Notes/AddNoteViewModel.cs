using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Entities;
using Organizer.Common.Enums;
using Organizer.Common.Exceptions;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class AddNoteViewModel : ViewModelBase
    {
        private Command _saveCommand;
        private Command _cancelCommand;
        private Note _note;
        private INoteService _noteService;

        public event EventHandler SaveMessage = delegate { };

        public event EventHandler CancelMessage = delegate { };

        public event EventHandler CheckValidationMessage = delegate { };

        public ICommand SaveCommand => _saveCommand;

        public ICommand CancelCommand => _cancelCommand;

        public bool IsModelValid { get; set; }

        public string HeaderText => "Add note";

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

        public AddNoteViewModel()
        {
            _noteService = App.Containter.Resolve<INoteService>();

            _note = new Note()
            {
                NoteType = NoteType.Diary,
                CreationDate = DateTime.Now,
                LastChangeDate = DateTime.Now,
                UserId = App.CurrentUser.Id
            };

            _saveCommand = Command.CreateCommand("Save note", "SaveCommand", GetType(), Save);
            _cancelCommand = Command.CreateCommand("Cancel", "CancelCommand", GetType(), Cancel);
        }

        private void Save()
        {
            CheckValidation();

            if (IsModelValid)
            {
                try
                {
                    _noteService.AddNote(_note);
                    SaveMessage.Invoke(null, EventArgs.Empty);
                }
                catch (NoteCaptionAlreadyExistsException e)
                {
                    MessageBox.Show($"Invalid data provided. Note cannot be saved.\nDetails: {e.Message}", "Error");
                }
                catch
                {
                    MessageBox.Show("Invalid data provided. Note cannot be saved.", "Error");
                }
            }
        }

        private void CheckValidation()
        {
            CheckValidationMessage.Invoke(null, EventArgs.Empty);
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