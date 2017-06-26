using Organizer.UI.Commands;
using System;
using System.Windows.Input;
using System.Windows;

namespace Organizer.UI.ViewModels
{
    public class StartupViewModel : ViewModelBase
    {
        private Command _openContactsCommand;
        private Command _openNotesCommand;
        private Command _openTodosCommand;
        private Command _openMeetingsCommand;
        private Command _closeCommand;

        public event EventHandler OpenContactsMessage = delegate { };

        public event EventHandler OpenNotesMessage = delegate { };

        public event EventHandler OpenTodosMessage = delegate { };

        public event EventHandler OpenMeetingsMessage = delegate { };

        public string CurrentUserText => "Current User:";

        public string CurrentUserName => App.CurrentUser.Login;

        public ICommand OpenContactsCommand => _openContactsCommand;
        public ICommand OpenNotesCommand => _openNotesCommand;
        public ICommand OpenTodosCommand => _openTodosCommand;
        public ICommand OpenMeetingsCommand => _openMeetingsCommand;
        public ICommand CloseCommand => _closeCommand;

        public StartupViewModel()
        {
            _openContactsCommand = Command.CreateCommand("Open contacts!", "OpenContacts", GetType(), null);
            _openNotesCommand = Command.CreateCommand("Open contacts!", "OpenContacts", GetType(), null);
            _openTodosCommand = Command.CreateCommand("Open contacts!", "OpenContacts", GetType(), null);
            _openMeetingsCommand = Command.CreateCommand("Open contacts!", "OpenContacts", GetType(), null);
            _closeCommand = Command.CreateCommand("Close", "Close", GetType(), () => App.Current.Shutdown());
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _openContactsCommand);
            Command.RegisterCommandBinding(window, _openContactsCommand);
            Command.RegisterCommandBinding(window, _openContactsCommand);
            Command.RegisterCommandBinding(window, _openContactsCommand);
            Command.RegisterCommandBinding(window, _closeCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
        }

        private void OpenContacts()
        {
            OpenContactsMessage.Invoke(null, EventArgs.Empty);
        }

        private void OpenNotes()
        {
            OpenNotesMessage.Invoke(null, EventArgs.Empty);
        }

        private void OpenTodos()
        {
            OpenTodosMessage.Invoke(null, EventArgs.Empty);
        }

        private void OpenMeetings()
        {
            OpenMeetingsMessage.Invoke(null, EventArgs.Empty);
        }
    }
}