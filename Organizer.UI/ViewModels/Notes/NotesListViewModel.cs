using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Enums;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class NotesListViewModel : ViewModelBase
    {
        private int _pageNumber;
        private const int _numberOnPage = 10;
        private INoteService _noteService;

        private ObservableCollection<NoteDto> _notes;
        private NoteDto _selected;
        private Command _addNoteCommand;
        private Command _deleteNoteCommand;
        private Command _editNoteCommand;
        private Command _viewNoteCommand;
        private Command _backCommand;
        private Command _fetchNextPageCommand;

        public event EventHandler AddNoteMessage = delegate { };

        public event EventHandler BackMessage = delegate { };

        public event EventHandler DeleteNoteMessage = delegate { };

        public event EventHandler EditNoteMessage = delegate { };

        public event EventHandler ViewNoteMessage = delegate { };

        public ICommand AddNoteCommand => _addNoteCommand;
        public ICommand DeleteNoteCommand => _deleteNoteCommand;
        public ICommand EditNoteCommand => _editNoteCommand;
        public ICommand ViewNoteCommand => _viewNoteCommand;
        public ICommand BackCommand => _backCommand;
        public ICommand FetchNextPageCommand => _fetchNextPageCommand;

        public ICollection<NoteDto> Notes => _notes;

        public NoteDto SelectedNote
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(SelectedNote));
            }
        }

        public NotesListViewModel()
        {
            _pageNumber = 1;
            _selected = null;

            _noteService = App.Containter.Resolve<INoteService>();

            var notesList = _noteService.GetNotesByNoteType(App.CurrentUser, NoteType.Diary, _numberOnPage, _pageNumber).ToList();

            _notes = new ObservableCollection<NoteDto>(notesList);

            _addNoteCommand = Command.CreateCommand("Add note", "AddNote", GetType(), AddNote);
            _deleteNoteCommand = Command.CreateCommand("Delete note", "DeleteNote", GetType(),
                DeleteNote, () => _selected != null);

            _editNoteCommand = Command.CreateCommand("Edit note", "EditNote", GetType(),
                EditNote, () => _selected != null);

            _viewNoteCommand = Command.CreateCommand("View note details", "ViewNote", GetType(),
                ViewNoteDetails, () => _selected != null);

            _fetchNextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage);
            _backCommand = Command.CreateCommand("Back to main menu", "BackCommand", GetType(), Back);
        }

        private void AddNote()
        {
            AddNoteMessage.Invoke(null, EventArgs.Empty);
        }

        private void DeleteNote()
        {
            var res = MessageBox.Show("Are you sure that you want to delete this note?",
                "Delete meeting confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    _noteService.RemoveNote(_selected);

                    var notesList = _noteService
                        .GetNotes(App.CurrentUser, _numberOnPage, _pageNumber).ToList();

                    _notes.Clear();

                    _notes = null;

                    _notes = new ObservableCollection<NoteDto>(notesList);

                    OnPropertyChanged(nameof(Notes));

                    DeleteNoteMessage.Invoke(null, EventArgs.Empty);
                }
                catch
                {
                    MessageBox.Show("Delete operation failed. An uncatched error occur\nwhile deleting a note.",
                        "Delete failed");
                }
            }
        }

        private void Back()
        {
            BackMessage.Invoke(null, EventArgs.Empty);
        }

        private void EditNote()
        {
            EditNoteMessage.Invoke(null, EventArgs.Empty);
        }

        private void ViewNoteDetails()
        {
            ViewNoteMessage.Invoke(null, EventArgs.Empty);
        }

        private void FetchNextPage()
        {
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _addNoteCommand);
            Command.RegisterCommandBinding(window, _deleteNoteCommand);
            Command.RegisterCommandBinding(window, _editNoteCommand);
            Command.RegisterCommandBinding(window, _viewNoteCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _fetchNextPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _addNoteCommand);
            Command.UnregisterCommandBinding(window, _deleteNoteCommand);
            Command.UnregisterCommandBinding(window, _editNoteCommand);
            Command.UnregisterCommandBinding(window, _viewNoteCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _fetchNextPageCommand);
        }
    }
}