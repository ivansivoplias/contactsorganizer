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
            _viewModel.UpdateViewValidation += SearchTypeChangedHandler;
            _viewModel.BackMessage += BackMessageHandler;
            _viewModel.ValidateSearch += ValidateSearchHandler;
            _viewModel.EditTodoMessage += EditTodoMessageHandler;
            _viewModel.ViewTodoMessage += ViewTodoMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();

            this.Title = _viewModel.HeaderText;
        }

        private void EditTodoMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var editTodoViewModel = new EditTodoViewModel(_viewModel.SelectedTodo);

                var editTodoWindow = new EditTodoWindow(editTodoViewModel);

                editTodoWindow.ShowInTaskbar = false;
                editTodoWindow.Owner = this;
                editTodoWindow.ShowDialog();
            });
        }

        private void ValidateSearchHandler(object sender, EventArgs e)
        {
            bool searchValid = !searchBox.GetBindingExpression(TextBox.TextProperty).HasError;
            bool searchNotNull = _viewModel.SearchType != Common.Enums.SearchTypes.TodoSearchType.Default ?
                !string.IsNullOrEmpty(_viewModel.SearchValue) : true;

            _viewModel.IsSearchValueValid = searchValid && searchNotNull;
        }

        private void ViewTodoMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewTodoViewModel = new ViewTodoViewModel(_viewModel.SelectedTodo);

                var viewTodoWindow = new ViewTodoWindow(viewTodoViewModel);

                viewTodoWindow.ShowInTaskbar = false;
                viewTodoWindow.Owner = this;
                viewTodoWindow.ShowDialog();
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
                addTodoWindow.ShowInTaskbar = false;
                addTodoWindow.Owner = this;
                addTodoWindow.ShowDialog();
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
            _viewModel.UpdateViewValidation -= SearchTypeChangedHandler;
            _viewModel.ValidateSearch -= ValidateSearchHandler;
            _viewModel.AddTodoMessage -= AddTodoMessageHandler;
            _viewModel.EditTodoMessage -= EditTodoMessageHandler;
            _viewModel.ViewTodoMessage -= ViewTodoMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }

        private void DataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            bool isBottom = IsScrollViewReachedTheBottom(e);
            if (isBottom)
            {
                _viewModel.NextPageCommand.Execute(null);
            }
        }

        private bool IsScrollViewReachedTheBottom(ScrollChangedEventArgs e)
        {
            if (e.ExtentHeight - e.ViewportHeight == 0 && e.VerticalOffset != 0)
                return true;
            if (e.VerticalOffset == 0)
                return false;
            if (e.ExtentHeight - e.ViewportHeight - e.VerticalOffset == 0)
                return true;
            return false;
        }
    }
}