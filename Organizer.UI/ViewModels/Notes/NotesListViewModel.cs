using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Entities;
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
        private int _totalCount;
        private INoteService _noteService;

        private DiarySearchType _currentSearchType;
        private string _searchValue;

        private ObservableCollection<Note> _notes;
        private Note _selected;
        private Command _searchCommand;
        private Command _addNoteCommand;
        private Command _deleteNoteCommand;
        private Command _editNoteCommand;
        private Command _viewNoteCommand;
        private Command _backCommand;

        private Command _nextPageCommand;
        private Command _previousPageCommand;
        private Command _firstPageCommand;
        private Command _lastPageCommand;

        public DiarySearchType SearchType
        {
            get { return _currentSearchType; }
            set
            {
                if (_currentSearchType == value)
                    return;
                _currentSearchType = value;
                OnPropertyChanged(nameof(SearchType));
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

        public string HeaderText => "Notes list";

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

        public ICommand NextPageCommand => _nextPageCommand;
        public ICommand PreviousPageCommand => _previousPageCommand;
        public ICommand FirstPageCommand => _firstPageCommand;
        public ICommand LastPageCommand => _lastPageCommand;

        public int PagesCount => (_totalCount + _numberOnPage - 1) / _numberOnPage;

        public int CurrentPage => _pageNumber;

        public ICollection<Note> Notes => _notes;

        public Note SelectedNote
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

            _totalCount = _noteService.GetNotesByNoteTypeCount(App.CurrentUser, NoteType.Diary);

            var notesList = _noteService.GetNotesByNoteType(App.CurrentUser, NoteType.Diary, _numberOnPage, _pageNumber).ToList();

            _notes = new ObservableCollection<Note>(notesList);

            _addNoteCommand = Command.CreateCommand("Add note", "AddNote", GetType(), AddNote);
            _deleteNoteCommand = Command.CreateCommand("Delete note", "DeleteNote", GetType(),
                DeleteNote, () => _selected != null);

            _editNoteCommand = Command.CreateCommand("Edit note", "EditNote", GetType(),
                EditNote, () => _selected != null);

            _searchCommand = Command.CreateCommand("Search", "Search", GetType(),
                Search);

            _viewNoteCommand = Command.CreateCommand("View note details", "ViewNote", GetType(),
                ViewNoteDetails, () => _selected != null);

            _nextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage, NextPageCanExecuted);
            _previousPageCommand = Command.CreateCommand("Previous page", "PreviousPage", GetType(), FetchPreviousPage, PreviousPageCanExecuted);
            _firstPageCommand = Command.CreateCommand("First page", "FirstPage", GetType(), FetchFirstPage, FirstPageCanExecuted);
            _lastPageCommand = Command.CreateCommand("Last page", "LastPage", GetType(), FetchLastPage, LastPageCanExecuted);
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
            UpdateViewValidationMessage.Invoke(null, EventArgs.Empty);
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                UpdateTotalCount();
                _pageNumber = 1;
                var list = FetchNotes(_pageNumber, _numberOnPage);
                _notes.Clear();
                _notes = null;

                _notes = new ObservableCollection<Note>(list);

                OnPropertyChanged(nameof(Notes));
            }
        }

        private void RefreshList()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                UpdateTotalCount();
                var list = FetchNotes(_pageNumber, _numberOnPage);
                _notes.Clear();
                _notes = null;

                _notes = new ObservableCollection<Note>(list);

                OnPropertyChanged(nameof(Notes));
            }
        }

        private void CheckSearchValidation()
        {
            ValidateSearch.Invoke(null, EventArgs.Empty);
        }

        #region Pagination

        private void FetchNextPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber++;
                var notes = FetchNotes(_pageNumber, _numberOnPage);
                if (!notes.IsNullOrEmpty())
                {
                    _notes.Clear();
                    _notes = null;

                    _notes = new ObservableCollection<Note>(notes);

                    OnPropertyChanged(nameof(Notes));
                }
            }
        }

        private void FetchPreviousPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber--;
                var notes = FetchNotes(_pageNumber, _numberOnPage);
                if (!notes.IsNullOrEmpty())
                {
                    _notes.Clear();
                    _notes = null;

                    _notes = new ObservableCollection<Note>(notes);

                    OnPropertyChanged(nameof(Notes));
                }
            }
        }

        private void FetchFirstPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber = 1;
                var notes = FetchNotes(_pageNumber, _numberOnPage);
                if (!notes.IsNullOrEmpty())
                {
                    _notes.Clear();
                    _notes = null;

                    _notes = new ObservableCollection<Note>(notes);

                    OnPropertyChanged(nameof(Notes));
                }
            }
        }

        private void FetchLastPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber = PagesCount;
                var notes = FetchNotes(_pageNumber, _numberOnPage);
                if (!notes.IsNullOrEmpty())
                {
                    _notes.Clear();
                    _notes = null;

                    _notes = new ObservableCollection<Note>(notes);

                    OnPropertyChanged(nameof(Notes));
                }
            }
        }

        private bool NextPageCanExecuted()
        {
            return _pageNumber + 1 <= PagesCount;
        }

        private bool PreviousPageCanExecuted()
        {
            return _pageNumber > 1;
        }

        private bool FirstPageCanExecuted()
        {
            return _pageNumber != 1 && PagesCount != 0;
        }

        private bool LastPageCanExecuted()
        {
            return _pageNumber != PagesCount && PagesCount != 0;
        }

        private void UpdateTotalCount()
        {
            switch (_currentSearchType)
            {
                case DiarySearchType.ByCaptionLike:
                    _totalCount = _noteService.GetNotesByCaptionLikeCount(App.CurrentUser, _searchValue, NoteType.Diary);
                    break;

                case DiarySearchType.CreatedBetween:
                    var dates = _searchValue.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim()).ToArray();
                    var startDate = DateTime.Parse(dates[0]);
                    var endDate = DateTime.Parse(dates[1]);
                    _totalCount = _noteService
                        .GetNotesCreatedBetweenCount(App.CurrentUser, startDate, endDate, NoteType.Diary);
                    break;

                default:
                    _totalCount = _noteService
                        .GetNotesByNoteTypeCount(App.CurrentUser, NoteType.Diary);
                    break;
            }
        }

        private List<Note> FetchNotes(int page, int pageSize)
        {
            List<Note> result;

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

        #endregion Pagination

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _searchCommand);
            Command.RegisterCommandBinding(window, _addNoteCommand);
            Command.RegisterCommandBinding(window, _deleteNoteCommand);
            Command.RegisterCommandBinding(window, _editNoteCommand);
            Command.RegisterCommandBinding(window, _viewNoteCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _nextPageCommand);
            Command.RegisterCommandBinding(window, _previousPageCommand);
            Command.RegisterCommandBinding(window, _firstPageCommand);
            Command.RegisterCommandBinding(window, _lastPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _searchCommand);
            Command.UnregisterCommandBinding(window, _addNoteCommand);
            Command.UnregisterCommandBinding(window, _deleteNoteCommand);
            Command.UnregisterCommandBinding(window, _editNoteCommand);
            Command.UnregisterCommandBinding(window, _viewNoteCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _nextPageCommand);
            Command.UnregisterCommandBinding(window, _previousPageCommand);
            Command.UnregisterCommandBinding(window, _firstPageCommand);
            Command.UnregisterCommandBinding(window, _lastPageCommand);
        }
    }
}