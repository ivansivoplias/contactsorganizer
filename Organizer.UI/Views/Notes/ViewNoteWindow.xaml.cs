using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for ViewNoteWindow.xaml
    /// </summary>
    public partial class ViewNoteWindow : Window
    {
        private NoteDetailsViewModel _viewModel;

        public ViewNoteWindow(NoteDetailsViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.BackMessage += BackMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();
        }

        private void BackMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewModel = new NotesListViewModel();
                var notesList = new NotesListWindow(viewModel);
                notesList.Show();
                this.Close();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.BackMessage -= BackMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}