using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Enums;
using Organizer.Common.Enums.SearchTypes;
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
        private INoteService _noteService;
        private TodoSearchType _currentSearchType;
        private string _searchValue;

        private ObservableCollection<NoteDto> _todoNotes;
        private NoteDto _selected;
        private Command _searchCommand;
        private Command _addTodoCommand;
        private Command _deleteTodoCommand;
        private Command _editTodoCommand;
        private Command _viewTodoCommand;
        private Command _backCommand;
        private Command _fetchNextPageCommand;

        public TodoSearchType SearchType
        {
            get { return _currentSearchType; }
            set
            {
                if (_currentSearchType == value)
                    return;
                _currentSearchType = value;
                OnPropertyChanged(nameof(SearchType));
                SearchTypeChanged.Invoke(null, EventArgs.Empty);
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

        public event EventHandler SearchTypeChanged = delegate { };

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
        public ICommand FetchNextPageCommand => _fetchNextPageCommand;

        public ICollection<NoteDto> Todos => _todoNotes;

        public NoteDto SelectedTodo
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

            var notesList = _noteService
                .GetNotesByNoteType(App.CurrentUser, NoteType.Todo, _numberOnPage, _pageNumber).ToList();

            _todoNotes = new ObservableCollection<NoteDto>(notesList);

            _addTodoCommand = Command.CreateCommand("Add todo", "AddTodo", GetType(), AddTodo);
            _deleteTodoCommand = Command.CreateCommand("Delete todo", "DeleteTodo", GetType(),
                DeleteTodo, () => _selected != null);

            _editTodoCommand = Command.CreateCommand("Edit todo", "EditTodo", GetType(),
                EditTodo, () => _selected != null);

            _searchCommand = Command.CreateCommand("Search", "Search", GetType(),
                Search);

            _viewTodoCommand = Command.CreateCommand("View todo details", "ViewTodo", GetType(),
                ViewTodoDetails, () => _selected != null);

            _fetchNextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage);
            _backCommand = Command.CreateCommand("Back to main menu", "BackCommand", GetType(), Back);
        }

        private void AddTodo()
        {
            AddTodoMessage.Invoke(null, EventArgs.Empty);
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

                    var notesList = SearchNotes();

                    _todoNotes.Clear();

                    _todoNotes = null;

                    _todoNotes = new ObservableCollection<NoteDto>(notesList);

                    OnPropertyChanged(nameof(Todos));

                    DeleteTodoMessage.Invoke(null, EventArgs.Empty);
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
        }

        private void ViewTodoDetails()
        {
            ViewTodoMessage.Invoke(null, EventArgs.Empty);
        }

        private void Search()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber = 1;
                var list = SearchNotes();
                _todoNotes.Clear();
                _todoNotes = null;

                _todoNotes = new ObservableCollection<NoteDto>(list);
            }
        }

        private void CheckSearchValidation()
        {
            ValidateSearch.Invoke(null, EventArgs.Empty);
        }

        private void FetchNextPage()
        {
            _pageNumber++;
            var notes = SearchNotes();
            notes = _todoNotes.Union(notes).ToList();

            _todoNotes.Clear();
            _todoNotes = null;

            _todoNotes = new ObservableCollection<NoteDto>(notes);
        }

        private List<NoteDto> SearchNotes()
        {
            List<NoteDto> result;

            switch (_currentSearchType)
            {
                case TodoSearchType.ByState:
                    var state = (State)Enum.Parse(typeof(State), _searchValue);
                    result = _noteService
                        .GetNotesByCurrentState(App.CurrentUser, state, NoteType.Todo, _numberOnPage, _pageNumber)
                        .ToList();
                    break;

                case TodoSearchType.ByPriority:
                    var priority = (Priority)Enum.Parse(typeof(Priority), _searchValue);
                    result = _noteService
                        .GetNotesByPriority(App.CurrentUser, priority, NoteType.Todo, _numberOnPage, _pageNumber)
                        .ToList();
                    break;

                case TodoSearchType.CreatedBetween:
                    var dates = _searchValue.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim()).ToArray();
                    var startDate = DateTime.Parse(dates[0]);
                    var endDate = DateTime.Parse(dates[1]);
                    result = _noteService
                        .GetNotesCreatedBetween(App.CurrentUser, startDate, endDate, NoteType.Todo, _numberOnPage, _pageNumber)
                        .ToList();
                    break;

                case TodoSearchType.ByStartDate:
                    var start = DateTime.Parse(_searchValue);
                    result = _noteService
                        .GetNotesByStartDate(App.CurrentUser, start, NoteType.Todo, _numberOnPage, _pageNumber)
                        .ToList();
                    break;

                case TodoSearchType.ByEndDate:
                    var end = DateTime.Parse(_searchValue);
                    result = _noteService
                        .GetNotesByEndDate(App.CurrentUser, end, NoteType.Todo, _numberOnPage, _pageNumber)
                        .ToList();
                    break;

                default:
                    result = _noteService
                        .GetNotesByNoteType(App.CurrentUser, NoteType.Todo, _numberOnPage, _pageNumber).ToList();
                    break;
            }

            return result;
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _searchCommand);
            Command.RegisterCommandBinding(window, _addTodoCommand);
            Command.RegisterCommandBinding(window, _deleteTodoCommand);
            Command.RegisterCommandBinding(window, _editTodoCommand);
            Command.RegisterCommandBinding(window, _viewTodoCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _fetchNextPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _searchCommand);
            Command.UnregisterCommandBinding(window, _addTodoCommand);
            Command.UnregisterCommandBinding(window, _deleteTodoCommand);
            Command.UnregisterCommandBinding(window, _editTodoCommand);
            Command.UnregisterCommandBinding(window, _viewTodoCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _fetchNextPageCommand);
        }
    }
}