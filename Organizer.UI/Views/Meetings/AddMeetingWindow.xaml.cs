using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
            _viewModel.CheckValidationMessage += CheckValidationMessageHandler;
            _viewModel.SaveMessage += SaveMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);

            InitializeComponent();

            this.Title = _viewModel.HeaderText;
        }

        private void SaveMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckValidationMessageHandler(object sender, EventArgs e)
        {
            meetingTimeField.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateSource();
            meetingPlaceField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            meetingNameField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            descriptionField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            meetingDateField.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
            notificationDateField.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();

            bool isMeetingNameValid = !meetingNameField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isMeetingTimeValid = !meetingTimeField.GetBindingExpression(ComboBox.SelectedValueProperty).HasError;
            bool isMeetingPlaceValid = !meetingPlaceField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isDescriptionValid = !descriptionField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isMeetingDateValid = !meetingDateField.GetBindingExpression(DatePicker.SelectedDateProperty).HasError;
            bool isNotificationDateValid = !notificationDateField.GetBindingExpression(DatePicker.SelectedDateProperty).HasError;

            _viewModel.IsModelValid = isMeetingNameValid && isDescriptionValid
                && isMeetingDateValid && isNotificationDateValid && isMeetingPlaceValid && isMeetingTimeValid;
        }

        private void CancelMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.CancelMessage -= CancelMessageHandler;
            _viewModel.CheckValidationMessage -= CheckValidationMessageHandler;
            _viewModel.SaveMessage -= SaveMessageHandler;

            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}