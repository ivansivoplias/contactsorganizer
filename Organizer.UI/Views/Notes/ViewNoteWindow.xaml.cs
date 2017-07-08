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

            this.Title = _viewModel.HeaderText;
        }

        private void BackMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.BackMessage -= BackMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}