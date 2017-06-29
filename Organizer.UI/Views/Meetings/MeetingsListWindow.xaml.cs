using Organizer.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            _viewModel.BackMessage += BackMessageHandler;
            _viewModel.EditMeetingMessage += EditMeetingMessageHandler;
            _viewModel.ViewMeetingMessage += ViewMeetingMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();
        }

        private void EditMeetingMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var editMeetingViewModel = new EditMeetingViewModel(_viewModel.SelectedMeeting);

                var editMeetingWindow = new EditMeetingWindow(editMeetingViewModel);

                editMeetingWindow.Show();

                this.Close();
            });
        }

        private void ViewMeetingMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewMeetingViewModel = new MeetingDetailsViewModel(_viewModel.SelectedMeeting);

                var viewMeetingWindow = new ViewMeetingWindow(viewMeetingViewModel);

                viewMeetingWindow.Show();

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

        private void AddMeetingMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var addMeetingViewModel = new AddMeetingViewModel();

                var addMeetingWindow = new AddMeetingWindow(addMeetingViewModel);

                addMeetingWindow.Show();

                this.Close();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.BackMessage -= BackMessageHandler;
            _viewModel.AddMeetingMessage -= AddMeetingMessageHandler;
            _viewModel.EditMeetingMessage -= EditMeetingMessageHandler;
            _viewModel.ViewMeetingMessage -= ViewMeetingMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}