using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        private StartupViewModel _viewModel;

        public StartupWindow(StartupViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.OpenContactsMessage += OpenContactsMessageHandler;
            _viewModel.OpenMeetingsMessage += OpenMeetingsMessageHandler;
            _viewModel.OpenNotesMessage += OpenNotesMessageHandler;
            _viewModel.OpenTodosMessage += OpenTodosMessageHandler;
            _viewModel.LogoutMessage += LogoutMessageHandler;

            _viewModel.RegisterCommandsForWindow(this);

            this.DataContext = _viewModel;

            Closing += OnClosing;

            InitializeComponent();
        }

        private void OpenContactsMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewModel = new ContactsListViewModel();
                var contactsList = new ContactsListWindow(viewModel);
                contactsList.Show();
                this.Close();
            });
        }

        private void OpenNotesMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var notesList = new NotesListWindow();
                notesList.Show();
                this.Close();
            });
        }

        private void OpenTodosMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var todoList = new TodoListWindow();
                todoList.Show();
                this.Close();
            });
        }

        private void OpenMeetingsMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var meetingsWindow = new MeetingsListWindow();
                meetingsWindow.Show();
                this.Close();
            });
        }

        private void LogoutMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var loginViewModel = new LoginViewModel();
                var loginWindow = new LoginWindow(loginViewModel);
                loginWindow.Show();
                this.Close();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Closing -= OnClosing;

            _viewModel.OpenContactsMessage -= OpenContactsMessageHandler;
            _viewModel.OpenMeetingsMessage -= OpenMeetingsMessageHandler;
            _viewModel.OpenNotesMessage -= OpenNotesMessageHandler;
            _viewModel.OpenTodosMessage -= OpenTodosMessageHandler;
            _viewModel?.UnregisterCommandsForWindow(this);
        }
    }
}