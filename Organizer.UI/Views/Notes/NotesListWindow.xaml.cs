using Organizer.Common.Enums.SearchTypes;
using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
            _viewModel.UpdateViewValidationMessage += SearchTypeChangedHandler;
            _viewModel.ValidateSearch += ValidateSearchHandler;
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

                editNoteWindow.ShowDialog();
            });
        }

        private void ViewNoteMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewNoteViewModel = new NoteDetailsViewModel(_viewModel.SelectedNote);

                var viewNoteWindow = new ViewNoteWindow(viewNoteViewModel);

                viewNoteWindow.ShowDialog();
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

        private void ValidateSearchHandler(object sender, EventArgs e)
        {
            bool searchValid = !searchBox.GetBindingExpression(TextBox.TextProperty).HasError;
            bool searchNotNull = _viewModel.SearchType != DiarySearchType.Default ?
                !string.IsNullOrEmpty(_viewModel.SearchValue) : true;

            _viewModel.IsSearchValueValid = searchValid && searchNotNull;
        }

        private void SearchTypeChangedHandler(object sender, EventArgs e)
        {
            searchBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void AddNoteMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var addNoteViewModel = new AddNoteViewModel();

                var addNoteWindow = new AddNoteWindow(addNoteViewModel);

                addNoteWindow.ShowDialog();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.BackMessage -= BackMessageHandler;
            _viewModel.UpdateViewValidationMessage -= SearchTypeChangedHandler;
            _viewModel.ValidateSearch -= ValidateSearchHandler;
            _viewModel.AddNoteMessage -= AddNoteMessageHandler;
            _viewModel.EditNoteMessage -= EditNoteMessageHandler;
            _viewModel.ViewNoteMessage -= ViewNoteMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }

        private void DataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            bool isBottom = IsScrollViewReachedTheBottom(e);
            if (isBottom)
            {
                _viewModel.FetchNextPageCommand.Execute(null);
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