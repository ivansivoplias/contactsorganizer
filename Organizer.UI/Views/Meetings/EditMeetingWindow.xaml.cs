using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for EditMeetingWindow.xaml
    /// </summary>
    public partial class EditMeetingWindow : Window
    {
        private EditMeetingViewModel _viewModel;

        public EditMeetingWindow(EditMeetingViewModel viewModel)
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
            SetupMeetingsListForm();
        }

        private void CancelMessageHandler(object sender, EventArgs e)
        {
            SetupMeetingsListForm();
        }

        private void SetupMeetingsListForm()
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

            _viewModel.CancelMessage -= CancelMessageHandler;
            _viewModel.SaveMessage -= SaveMessageHandler;

            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}