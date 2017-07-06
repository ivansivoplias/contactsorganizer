using Organizer.Common.Enums.SearchTypes;
using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for MeetingsListWindow.xaml
    /// </summary>
    public partial class MeetingsListWindow : Window
    {
        private MeetingsListViewModel _viewModel;

        public MeetingsListWindow(MeetingsListViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.AddMeetingMessage += AddMeetingMessageHandler;
            _viewModel.UpdateViewValidationMessage += SearchTypeChangedHandler;
            _viewModel.ValidateSearch += ValidateSearchHandler;
            _viewModel.BackMessage += BackMessageHandler;
            _viewModel.EditMeetingMessage += EditMeetingMessageHandler;
            _viewModel.ViewMeetingMessage += ViewMeetingMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();

            this.Title = _viewModel.HeaderText;
        }

        private void EditMeetingMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var editMeetingViewModel = new EditMeetingViewModel(_viewModel.SelectedMeeting);

                var editMeetingWindow = new EditMeetingWindow(editMeetingViewModel);

                editMeetingWindow.ShowDialog();
            });
        }

        private void ViewMeetingMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewMeetingViewModel = new MeetingDetailsViewModel(_viewModel.SelectedMeeting);

                var viewMeetingWindow = new ViewMeetingWindow(viewMeetingViewModel);

                viewMeetingWindow.ShowDialog();
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
            bool searchNotNull = _viewModel.SearchType != MeetingSearchType.Default ?
                !string.IsNullOrEmpty(_viewModel.SearchValue) : true;

            _viewModel.IsSearchValueValid = searchValid && searchNotNull;
        }

        private void SearchTypeChangedHandler(object sender, EventArgs e)
        {
            searchBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void AddMeetingMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var addMeetingViewModel = new AddMeetingViewModel();

                var addMeetingWindow = new AddMeetingWindow(addMeetingViewModel);

                addMeetingWindow.ShowDialog();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.BackMessage -= BackMessageHandler;
            _viewModel.AddMeetingMessage -= AddMeetingMessageHandler;
            _viewModel.UpdateViewValidationMessage -= SearchTypeChangedHandler;
            _viewModel.ValidateSearch -= ValidateSearchHandler;
            _viewModel.EditMeetingMessage -= EditMeetingMessageHandler;
            _viewModel.ViewMeetingMessage -= ViewMeetingMessageHandler;
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