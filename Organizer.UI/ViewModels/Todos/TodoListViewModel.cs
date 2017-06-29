using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Enums;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class TodoListViewModel : ViewModelBase
    {
        private int _pageNumber;
        private const int _numberOnPage = 10;
        private INoteService _noteService;

        private ObservableCollection<NoteDto> _todoNotes;
        private NoteDto _selected;
        private Command _addTodoCommand;
        private Command _deleteTodoCommand;
        private Command _editTodoCommand;
        private Command _viewTodoCommand;
        private Command _backCommand;
        private Command _fetchNextPageCommand;

        public event EventHandler AddNoteMessage = delegate { };

        public event EventHandler BackMessage = delegate { };

        public event EventHandler DeleteMeetingMessage = delegate { };

        public event EventHandler EditNoteMessage = delegate { };

        public event EventHandler ViewNoteMessage = delegate { };

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

            _noteService = App.Containter.Resolve<INoteService>();

            var notesList = _noteService
                .GetNotesByNoteType(App.CurrentUser, NoteType.Todo, _numberOnPage, _pageNumber).ToList();

            _todoNotes = new ObservableCollection<NoteDto>(notesList);

            _addTodoCommand = Command.CreateCommand("Add todo", "AddTodo", GetType(), AddTodo);
            _deleteTodoCommand = Command.CreateCommand("Delete todo", "DeleteTodo", GetType(),
                DeleteTodo, () => _selected != null);

            _editTodoCommand = Command.CreateCommand("Edit todo", "EditTodo", GetType(),
                EditTodo, () => _selected != null);

            _viewTodoCommand = Command.CreateCommand("View todo details", "ViewTodo", GetType(),
                ViewTodoDetails, () => _selected != null);

            _fetchNextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage);
            _backCommand = Command.CreateCommand("Back to main menu", "BackCommand", GetType(), Back);
        }

        private void AddTodo()
        {
            AddNoteMessage.Invoke(null, EventArgs.Empty);
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

                    var notesList = _noteService
                        .GetNotesByNoteType(App.CurrentUser, NoteType.Todo, _numberOnPage, _pageNumber).ToList();

                    _todoNotes.Clear();

                    _todoNotes = null;

                    _todoNotes = new ObservableCollection<NoteDto>(notesList);

                    OnPropertyChanged(nameof(Todos));

                    DeleteMeetingMessage.Invoke(null, EventArgs.Empty);
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
            EditNoteMessage.Invoke(null, EventArgs.Empty);
        }

        private void ViewTodoDetails()
        {
            ViewNoteMessage.Invoke(null, EventArgs.Empty);
        }

        private void FetchNextPage()
        {
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _addTodoCommand);
            Command.RegisterCommandBinding(window, _deleteTodoCommand);
            Command.RegisterCommandBinding(window, _editTodoCommand);
            Command.RegisterCommandBinding(window, _viewTodoCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _fetchNextPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _addTodoCommand);
            Command.UnregisterCommandBinding(window, _deleteTodoCommand);
            Command.UnregisterCommandBinding(window, _editTodoCommand);
            Command.UnregisterCommandBinding(window, _viewTodoCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _fetchNextPageCommand);
        }
    }
}