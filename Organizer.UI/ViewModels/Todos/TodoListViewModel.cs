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
    public class TodoListViewModel : ViewModelBase
    {
        private int _pageNumber;
        private const int _numberOnPage = 10;
        private int _totalCount;
        private INoteService _noteService;
        private TodoSearchType _currentSearchType;
        private string _searchValue;

        private ObservableCollection<Note> _todoNotes;
        private Note _selected;
        private Command _searchCommand;
        private Command _addTodoCommand;
        private Command _deleteTodoCommand;
        private Command _editTodoCommand;
        private Command _viewTodoCommand;
        private Command _backCommand;

        private Command _nextPageCommand;
        private Command _previousPageCommand;
        private Command _firstPageCommand;
        private Command _lastPageCommand;

        public TodoSearchType SearchType
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

        public event EventHandler AddTodoMessage = delegate { };

        public event EventHandler UpdateViewValidation = delegate { };

        public event EventHandler BackMessage = delegate { };

        public event EventHandler DeleteTodoMessage = delegate { };

        public event EventHandler EditTodoMessage = delegate { };

        public event EventHandler ViewTodoMessage = delegate { };

        public event EventHandler ValidateSearch = delegate { };

        public ICommand SearchCommand => _searchCommand;
        public ICommand AddTodoCommand => _addTodoCommand;
        public ICommand DeleteTodoCommand => _deleteTodoCommand;
        public ICommand EditTodoCommand => _editTodoCommand;
        public ICommand ViewTodoCommand => _viewTodoCommand;
        public ICommand BackCommand => _backCommand;

        public ICommand NextPageCommand => _nextPageCommand;
        public ICommand PreviousPageCommand => _previousPageCommand;
        public ICommand FirstPageCommand => _firstPageCommand;
        public ICommand LastPageCommand => _lastPageCommand;

        public int PagesCount => PaginationHelper.GetPagesCount(_totalCount, _numberOnPage);

        public int CurrentPage => _pageNumber;

        public ICollection<Note> Todos => _todoNotes;

        public Note SelectedTodo
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(SelectedTodo));
            }
        }

        public TodoListViewModel()
        {
            _pageNumber = 1;
            _selected = null;
            _currentSearchType = TodoSearchType.Default;

            _noteService = App.Containter.Resolve<INoteService>();

            _totalCount = _noteService.GetNotesByNoteTypeCount(App.CurrentUser, NoteType.Todo);

            var notesList = _noteService
                .GetNotesByNoteType(App.CurrentUser, NoteType.Todo, _numberOnPage, _pageNumber).ToList();

            _todoNotes = new ObservableCollection<Note>(notesList);

            _addTodoCommand = Command.CreateCommand("Add todo", "AddTodo", GetType(), AddTodo);
            _deleteTodoCommand = Command.CreateCommand("Delete todo", "DeleteTodo", GetType(),
                DeleteTodo, () => _selected != null);

            _editTodoCommand = Command.CreateCommand("Edit todo", "EditTodo", GetType(),
                EditTodo, () => _selected != null);

            _searchCommand = Command.CreateCommand("Search", "Search", GetType(),
                Search);

            _viewTodoCommand = Command.CreateCommand("View todo details", "ViewTodo", GetType(),
                ViewTodoDetails, () => _selected != null);

            _nextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage, NextPageCanExecuted);
            _previousPageCommand = Command.CreateCommand("Previous page", "PreviousPage", GetType(), FetchPreviousPage, PreviousPageCanExecuted);
            _firstPageCommand = Command.CreateCommand("First page", "FirstPage", GetType(), FetchFirstPage, FirstPageCanExecuted);
            _lastPageCommand = Command.CreateCommand("Last page", "LastPage", GetType(), FetchLastPage, LastPageCanExecuted);
            _backCommand = Command.CreateCommand("Back to main menu", "BackCommand", GetType(), Back);
        }

        private void AddTodo()
        {
            AddTodoMessage.Invoke(null, EventArgs.Empty);
            RefreshList();
        }

        private void DeleteTodo()
        {
            var res = MessageBox.Show("Are you sure that you want to delete this todo item?",
                "Delete todo item confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    _noteService.RemoveNote(_selected);

                    DeleteTodoMessage.Invoke(null, EventArgs.Empty);

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

        private void EditTodo()
        {
            EditTodoMessage.Invoke(null, EventArgs.Empty);
            RefreshList();
        }

        private void ViewTodoDetails()
        {
            ViewTodoMessage.Invoke(null, EventArgs.Empty);
        }

        private void Search()
        {
            UpdateViewValidation.Invoke(null, EventArgs.Empty);
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                UpdateTotalCount();
                _pageNumber = 1;
                var list = FetchNotes(_pageNumber, _numberOnPage);
                _todoNotes.Clear();
                _todoNotes = null;

                _todoNotes = new ObservableCollection<Note>(list);

                OnPropertyChanged(nameof(Todos));
            }
        }

        private void RefreshList()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                UpdateTotalCount();
                var list = FetchNotes(_pageNumber, _numberOnPage);
                _todoNotes.Clear();
                _todoNotes = null;

                _todoNotes = new ObservableCollection<Note>(list);

                OnPropertyChanged(nameof(Todos));
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
                    _todoNotes.Clear();
                    _todoNotes = null;

                    _todoNotes = new ObservableCollection<Note>(notes);

                    OnPropertyChanged(nameof(Todos));
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
                    _todoNotes.Clear();
                    _todoNotes = null;

                    _todoNotes = new ObservableCollection<Note>(notes);

                    OnPropertyChanged(nameof(Todos));
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
                    _todoNotes.Clear();
                    _todoNotes = null;

                    _todoNotes = new ObservableCollection<Note>(notes);

                    OnPropertyChanged(nameof(Todos));
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
                    _todoNotes.Clear();
                    _todoNotes = null;

                    _todoNotes = new ObservableCollection<Note>(notes);

                    OnPropertyChanged(nameof(Todos));
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
                case TodoSearchType.ByCaptionLike:
                    _totalCount = _noteService.GetNotesByCaptionLikeCount(App.CurrentUser, _searchValue, NoteType.Todo);
                    break;

                case TodoSearchType.ByState:
                    var state = (State)Enum.Parse(typeof(State), _searchValue);
                    _totalCount = _noteService
                        .GetNotesByCurrentStateCount(App.CurrentUser, state, NoteType.Todo);
                    break;

                case TodoSearchType.ByPriority:
                    var priority = (Priority)Enum.Parse(typeof(Priority), _searchValue);
                    _totalCount = _noteService
                        .GetNotesByPriorityCount(App.CurrentUser, priority, NoteType.Todo);
                    break;

                case TodoSearchType.CreatedBetween:
                    var dates = _searchValue.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim()).ToArray();
                    var startDate = DateTime.Parse(dates[0]);
                    var endDate = DateTime.Parse(dates[1]);
                    _totalCount = _noteService
                        .GetNotesCreatedBetweenCount(App.CurrentUser, startDate, endDate, NoteType.Todo);
                    break;

                case TodoSearchType.ByStartDate:
                    var start = DateTime.Parse(_searchValue);
                    _totalCount = _noteService
                        .GetNotesByStartDateCount(App.CurrentUser, start, NoteType.Todo);
                    break;

                case TodoSearchType.ByEndDate:
                    var end = DateTime.Parse(_searchValue);
                    _totalCount = _noteService
                        .GetNotesByEndDateCount(App.CurrentUser, end, NoteType.Todo);
                    break;

                default:
                    _totalCount = _noteService
                        .GetNotesByNoteTypeCount(App.CurrentUser, NoteType.Todo);
                    break;
            }
        }

        private List<Note> FetchNotes(int page, int pageSize)
        {
            List<Note> result;

            switch (_currentSearchType)
            {
                case TodoSearchType.ByCaptionLike:
                    result = _noteService.GetNotesByCaptionLike(App.CurrentUser, _searchValue, NoteType.Todo, pageSize, page)
                        .ToList();
                    break;

                case TodoSearchType.ByState:
                    var state = (State)Enum.Parse(typeof(State), _searchValue);
                    result = _noteService
                        .GetNotesByCurrentState(App.CurrentUser, state, NoteType.Todo, pageSize, page)
                        .ToList();
                    break;

                case TodoSearchType.ByPriority:
                    var priority = (Priority)Enum.Parse(typeof(Priority), _searchValue);
                    result = _noteService
                        .GetNotesByPriority(App.CurrentUser, priority, NoteType.Todo, pageSize, page)
                        .ToList();
                    break;

                case TodoSearchType.CreatedBetween:
                    var dates = _searchValue.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim()).ToArray();
                    var startDate = DateTime.Parse(dates[0]);
                    var endDate = DateTime.Parse(dates[1]);
                    result = _noteService
                        .GetNotesCreatedBetween(App.CurrentUser, startDate, endDate, NoteType.Todo, pageSize, page)
                        .ToList();
                    break;

                case TodoSearchType.ByStartDate:
                    var start = DateTime.Parse(_searchValue);
                    result = _noteService
                        .GetNotesByStartDate(App.CurrentUser, start, NoteType.Todo, pageSize, page)
                        .ToList();
                    break;

                case TodoSearchType.ByEndDate:
                    var end = DateTime.Parse(_searchValue);
                    result = _noteService
                        .GetNotesByEndDate(App.CurrentUser, end, NoteType.Todo, pageSize, page)
                        .ToList();
                    break;

                default:
                    result = _noteService
                        .GetNotesByNoteType(App.CurrentUser, NoteType.Todo, pageSize, page).ToList();
                    break;
            }

            return result;
        }

        #endregion Pagination

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _searchCommand);
            Command.RegisterCommandBinding(window, _addTodoCommand);
            Command.RegisterCommandBinding(window, _deleteTodoCommand);
            Command.RegisterCommandBinding(window, _editTodoCommand);
            Command.RegisterCommandBinding(window, _viewTodoCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _nextPageCommand);
            Command.RegisterCommandBinding(window, _previousPageCommand);
            Command.RegisterCommandBinding(window, _firstPageCommand);
            Command.RegisterCommandBinding(window, _lastPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _searchCommand);
            Command.UnregisterCommandBinding(window, _addTodoCommand);
            Command.UnregisterCommandBinding(window, _deleteTodoCommand);
            Command.UnregisterCommandBinding(window, _editTodoCommand);
            Command.UnregisterCommandBinding(window, _viewTodoCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _nextPageCommand);
            Command.UnregisterCommandBinding(window, _previousPageCommand);
            Command.UnregisterCommandBinding(window, _firstPageCommand);
            Command.UnregisterCommandBinding(window, _lastPageCommand);
        }
    }
}