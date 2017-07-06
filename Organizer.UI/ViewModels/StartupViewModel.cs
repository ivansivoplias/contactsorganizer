using Organizer.UI.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class StartupViewModel : ViewModelBase
    {
        private Command _openContactsCommand;
        private Command _openNotesCommand;
        private Command _openTodosCommand;
        private Command _openMeetingsCommand;
        private Command _closeCommand;
        private Command _logOutCommand;

        public event EventHandler OpenContactsMessage = delegate { };

        public event EventHandler OpenNotesMessage = delegate { };

        public event EventHandler OpenTodosMessage = delegate { };

        public event EventHandler OpenMeetingsMessage = delegate { };

        public event EventHandler LogoutMessage = delegate { };

        public string HeaderText => "Startup page";

        public string CurrentUserText => "Current User:";

        public string CurrentUserName => App.CurrentUser.Login;

        public ICommand OpenContactsCommand => _openContactsCommand;
        public ICommand OpenNotesCommand => _openNotesCommand;
        public ICommand OpenTodosCommand => _openTodosCommand;
        public ICommand OpenMeetingsCommand => _openMeetingsCommand;
        public ICommand CloseCommand => _closeCommand;
        public ICommand LogoutCommand => _logOutCommand;

        public StartupViewModel()
        {
            _openContactsCommand = Command.CreateCommand("Open contacts list", "OpenContacts", GetType(), OpenContacts);
            _openNotesCommand = Command.CreateCommand("Open notes list", "OpenNotes", GetType(), OpenNotes);
            _openTodosCommand = Command.CreateCommand("Open todo list", "OpenTodos", GetType(), OpenTodos);
            _openMeetingsCommand = Command.CreateCommand("Open meetings list", "OpenMeetings", GetType(), OpenMeetings);
            _closeCommand = Command.CreateCommand("Close", "Close", GetType(), () => App.Current.Shutdown());
            _logOutCommand = Command.CreateCommand("Log out", "Logout", GetType(), Logout);
        }

        private void Logout()
        {
            var settings = Properties.Settings.Default;
            settings.IsUserLoggedIn = false;
            settings.UserLogin = "";
            settings.UserPassword = "";
            settings.Save();
            LogoutMessage.Invoke(null, EventArgs.Empty);
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _openContactsCommand);
            Command.RegisterCommandBinding(window, _openNotesCommand);
            Command.RegisterCommandBinding(window, _openTodosCommand);
            Command.RegisterCommandBinding(window, _openMeetingsCommand);
            Command.RegisterCommandBinding(window, _logOutCommand);
            Command.RegisterCommandBinding(window, _closeCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _openContactsCommand);
            Command.UnregisterCommandBinding(window, _openNotesCommand);
            Command.UnregisterCommandBinding(window, _openTodosCommand);
            Command.UnregisterCommandBinding(window, _openMeetingsCommand);
            Command.UnregisterCommandBinding(window, _logOutCommand);
            Command.UnregisterCommandBinding(window, _closeCommand);
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