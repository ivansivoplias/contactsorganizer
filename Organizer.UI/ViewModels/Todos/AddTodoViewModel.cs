using Autofac;
using Organizer.Common.Entities;
using Organizer.Common.Enums;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class AddTodoViewModel : ViewModelBase
    {
        private Command _saveCommand;
        private Command _cancelCommand;
        private Note _note;
        private INoteService _noteService;
        private List<string> _priorities;
        private List<string> _states;

        public event EventHandler SaveMessage = delegate { };

        public event EventHandler CancelMessage = delegate { };

        public event EventHandler CheckValidationMessage = delegate { };

        public ICommand SaveCommand => _saveCommand;

        public ICommand CancelCommand => _cancelCommand;

        public bool IsModelValid { get; set; }

        public string HeaderText => "Add todo";

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

        public string State
        {
            get { return _note.State?.ToString(); }
            set
            {
                State state;
                if (Enum.TryParse(value, out state))
                {
                    _note.State = state;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        public string Priority
        {
            get { return _note.Priority?.ToString(); }
            set
            {
                Priority priority;
                if (Enum.TryParse(value, out priority))
                {
                    _note.Priority = priority;
                    OnPropertyChanged(nameof(Priority));
                }
            }
        }

        public DateTime? StartDate
        {
            get { return _note.StartDate; }
            set
            {
                _note.StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime? EndDate
        {
            get { return _note.EndDate; }
            set
            {
                _note.EndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public ICollection<string> Priorities => _priorities;

        public ICollection<string> States => _states;

        public AddTodoViewModel()
        {
            _states = (Enum.GetValues(typeof(State)) as State[])
                .Where(x => x != Common.Enums.State.None)
                .Select(x => x.ToString()).ToList();

            _priorities = (Enum.GetValues(typeof(Priority)) as Priority[])
                .Where(x => x != Common.Enums.Priority.None)
                .Select(x => x.ToString()).ToList();

            _noteService = App.Containter.Resolve<INoteService>();

            _note = new Note()
            {
                NoteType = NoteType.Todo,
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
                catch
                {
                    MessageBox.Show("Invalid data provided. Todo cannot be saved.", "Error");
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