using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Enums;
using Organizer.Common.Enums.SearchTypes;
using Organizer.Common.Helpers;
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

        private DiarySearchType _currentSearchType;
        private string _searchValue;

        private ObservableCollection<NoteDto> _notes;
        private NoteDto _selected;
        private Command _searchCommand;
        private Command _addNoteCommand;
        private Command _deleteNoteCommand;
        private Command _editNoteCommand;
        private Command _viewNoteCommand;
        private Command _backCommand;
        private Command _fetchNextPageCommand;

        public DiarySearchType SearchType
        {
            get { return _currentSearchType; }
            set
            {
                if (_currentSearchType == value)
                    return;
                _currentSearchType = value;
                OnPropertyChanged(nameof(SearchType));
                //SearchTypeChanged.Invoke(null, EventArgs.Empty);
            }
        }

        public string SearchValue
        {
            get { return _searchValue; }
            set
            {
                _searchValue = value;
                OnPropertyChanged(nameof(SearchValue));
            }
        }

        public bool IsSearchValueValid { get; set; }

        public event EventHandler AddNoteMessage = delegate { };

        public event EventHandler UpdateViewValidationMessage = delegate { };

        public event EventHandler BackMessage = delegate { };

        public event EventHandler DeleteNoteMessage = delegate { };

        public event EventHandler EditNoteMessage = delegate { };

        public event EventHandler ViewNoteMessage = delegate { };

        public event EventHandler ValidateSearch = delegate { };

        public ICommand SearchCommand => _searchCommand;
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
            _currentSearchType = DiarySearchType.Default;

            _noteService = App.Containter.Resolve<INoteService>();

            var notesList = _noteService.GetNotesByNoteType(App.CurrentUser, NoteType.Diary, _numberOnPage, _pageNumber).ToList();

            _notes = new ObservableCollection<NoteDto>(notesList);

            _addNoteCommand = Command.CreateCommand("Add note", "AddNote", GetType(), AddNote);
            _deleteNoteCommand = Command.CreateCommand("Delete note", "DeleteNote", GetType(),
                DeleteNote, () => _selected != null);

            _editNoteCommand = Command.CreateCommand("Edit note", "EditNote", GetType(),
                EditNote, () => _selected != null);

            _searchCommand = Command.CreateCommand("Search", "Search", GetType(),
                Search);

            _viewNoteCommand = Command.CreateCommand("View note details", "ViewNote", GetType(),
                ViewNoteDetails, () => _selected != null);

            _fetchNextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage);
            _backCommand = Command.CreateCommand("Back to main menu", "BackCommand", GetType(), Back);
        }

        private void AddNote()
        {
            AddNoteMessage.Invoke(null, EventArgs.Empty);
            RefreshList();
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

                    DeleteNoteMessage.Invoke(null, EventArgs.Empty);

                    RefreshList();
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
            RefreshList();
        }

        private void ViewNoteDetails()
        {
            ViewNoteMessage.Invoke(null, EventArgs.Empty);
        }

        private void Search()
        {
            CheckSearchValidation();
            UpdateViewValidationMessage.Invoke(null, EventArgs.Empty);
            if (IsSearchValueValid)
            {
                _pageNumber = 1;
                var list = FetchNotes(_pageNumber, _numberOnPage);
                _notes.Clear();
                _notes = null;

                _notes = new ObservableCollection<NoteDto>(list);

                OnPropertyChanged(nameof(Notes));
            }
        }

        private void RefreshList()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                var list = FetchNotes(1, _numberOnPage * _pageNumber);
                _notes.Clear();
                _notes = null;

                _notes = new ObservableCollection<NoteDto>(list);

                OnPropertyChanged(nameof(Notes));
            }
        }

        private void CheckSearchValidation()
        {
            ValidateSearch.Invoke(null, EventArgs.Empty);
        }

        private void FetchNextPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber++;
                var notes = FetchNotes(_pageNumber, _numberOnPage);
                if (!notes.IsNullOrEmpty())
                {
                    notes = _notes.Union(notes).ToList();

                    _notes.Clear();
                    _notes = null;

                    _notes = new ObservableCollection<NoteDto>(notes);

                    OnPropertyChanged(nameof(Notes));
                }
                else
                {
                    _pageNumber--;
                }
            }
        }

        private List<NoteDto> FetchNotes(int page, int pageSize)
        {
            List<NoteDto> result;

            switch (_currentSearchType)
            {
                case DiarySearchType.ByCaptionLike:
                    result = _noteService.GetNotesByCaptionLike(App.CurrentUser, _searchValue, NoteType.Diary, pageSize, page)
                        .ToList();
                    break;

                case DiarySearchType.CreatedBetween:
                    var dates = _searchValue.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim()).ToArray();
                    var startDate = DateTime.Parse(dates[0]);
                    var endDate = DateTime.Parse(dates[1]);
                    result = _noteService
                        .GetNotesCreatedBetween(App.CurrentUser, startDate, endDate, NoteType.Diary, pageSize, page)
                        .ToList();
                    break;

                default:
                    result = _noteService
                        .GetNotesByNoteType(App.CurrentUser, NoteType.Diary, pageSize, page).ToList();
                    break;
            }

            return result;
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _searchCommand);
            Command.RegisterCommandBinding(window, _addNoteCommand);
            Command.RegisterCommandBinding(window, _deleteNoteCommand);
            Command.RegisterCommandBinding(window, _editNoteCommand);
            Command.RegisterCommandBinding(window, _viewNoteCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _fetchNextPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _searchCommand);
            Command.UnregisterCommandBinding(window, _addNoteCommand);
            Command.UnregisterCommandBinding(window, _deleteNoteCommand);
            Command.UnregisterCommandBinding(window, _editNoteCommand);
            Command.UnregisterCommandBinding(window, _viewNoteCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _fetchNextPageCommand);
        }
    }
}