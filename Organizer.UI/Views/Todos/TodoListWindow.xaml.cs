using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for TodoListWindow.xaml
    /// </summary>
    public partial class TodoListWindow : Window
    {
        private TodoListViewModel _viewModel;

        public TodoListWindow(TodoListViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.AddTodoMessage += AddTodoMessageHandler;
            _viewModel.SearchTypeChanged += SearchTypeChangedHandler;
            _viewModel.BackMessage += BackMessageHandler;
            _viewModel.EditTodoMessage += EditTodoMessageHandler;
            _viewModel.ViewTodoMessage += ViewTodoMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();
        }

        private void EditTodoMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var editTodoViewModel = new EditTodoViewModel(_viewModel.SelectedTodo);

                var editTodoWindow = new EditTodoWindow(editTodoViewModel);

                editTodoWindow.Show();

                this.Close();
            });
        }

        private void ViewTodoMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewTodoViewModel = new ViewTodoViewModel(_viewModel.SelectedTodo);

                var viewTodoWindow = new ViewTodoWindow(viewTodoViewModel);

                viewTodoWindow.Show();

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

        private void AddTodoMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var addTodoViewModel = new AddTodoViewModel();

                var addTodoWindow = new AddTodoWindow(addTodoViewModel);

                addTodoWindow.Show();

                this.Close();
            });
        }

        private void SearchTypeChangedHandler(object sender, EventArgs e)
        {
            searchBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.BackMessage -= BackMessageHandler;
            _viewModel.SearchTypeChanged -= SearchTypeChangedHandler;
            _viewModel.AddTodoMessage -= AddTodoMessageHandler;
            _viewModel.EditTodoMessage -= EditTodoMessageHandler;
            _viewModel.ViewTodoMessage -= ViewTodoMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}