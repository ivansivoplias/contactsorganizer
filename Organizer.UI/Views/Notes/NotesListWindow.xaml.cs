using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for NotesListWindow.xaml
    /// </summary>
    public partial class NotesListWindow : Window
    {
        private NotesListViewModel _viewModel;

        public NotesListWindow(NotesListViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.AddNoteMessage += AddNoteMessageHandler;
            _viewModel.BackMessage += BackMessageHandler;
            _viewModel.EditNoteMessage += EditNoteMessageHandler;
            _viewModel.ViewNoteMessage += ViewNoteMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();
        }

        private void EditNoteMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var editNoteViewModel = new EditNoteViewModel(_viewModel.SelectedNote);

                var editNoteWindow = new EditNoteWindow(editNoteViewModel);

                editNoteWindow.Show();

                this.Close();
            });
        }

        private void ViewNoteMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewNoteViewModel = new NoteDetailsViewModel(_viewModel.SelectedNote);

                var viewNoteWindow = new ViewNoteWindow(viewNoteViewModel);

                viewNoteWindow.Show();

                this.Close();
            });
        }

        private void BackMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var startupViewModel = new StartupViewModel();

                var startupWindow = new StartupWindow(startupViewModel);

                startupWindow.Show();

                this.Close();
            });
        }

        private void AddNoteMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var addNoteViewModel = new AddNoteViewModel();

                var addNoteWindow = new AddNoteWindow(addNoteViewModel);

                addNoteWindow.Show();

                this.Close();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.BackMessage -= BackMessageHandler;
            _viewModel.AddNoteMessage -= AddNoteMessageHandler;
            _viewModel.EditNoteMessage -= EditNoteMessageHandler;
            _viewModel.ViewNoteMessage -= ViewNoteMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}