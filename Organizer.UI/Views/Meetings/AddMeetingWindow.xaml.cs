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
    /// Interaction logic for AddMeetingWindow.xaml
    /// </summary>
    public partial class AddMeetingWindow : Window
    {
        private AddMeetingViewModel _viewModel;

        public AddMeetingWindow(AddMeetingViewModel viewModel)
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