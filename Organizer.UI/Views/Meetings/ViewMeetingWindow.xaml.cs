using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for ViewMeetingWindow.xaml
    /// </summary>
    public partial class ViewMeetingWindow : Window
    {
        private MeetingDetailsViewModel _viewModel;

        public ViewMeetingWindow(MeetingDetailsViewModel viewModel)
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
                var viewModel = new MeetingsListViewModel();
                var meetingsList = new MeetingsListWindow(viewModel);
                meetingsList.Show();
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