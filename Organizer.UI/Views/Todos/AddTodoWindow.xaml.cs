using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for AddTodoWindow.xaml
    /// </summary>
    public partial class AddTodoWindow : Window
    {
        private AddTodoViewModel _viewModel;

        public AddTodoWindow(AddTodoViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.CancelMessage += CancelMessageHandler;
            _viewModel.SaveMessage += SaveMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);

            InitializeComponent();
        }

        private void SaveMessageHandler(object sender, EventArgs e)
        {
            SetupNotesListForm();
        }

        private void CancelMessageHandler(object sender, EventArgs e)
        {
            SetupNotesListForm();
        }

        private void SetupNotesListForm()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewModel = new TodoListViewModel();
                var todosList = new TodoListWindow(viewModel);
                todosList.Show();
                this.Close();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.CancelMessage -= CancelMessageHandler;
            _viewModel.SaveMessage -= SaveMessageHandler;

            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}