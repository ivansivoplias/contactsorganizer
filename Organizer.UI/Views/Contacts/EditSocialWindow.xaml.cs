using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for EditContactWindow.xaml
    /// </summary>
    public partial class EditSocialWindow : Window
    {
        private EditSocialViewModel _viewModel;

        public EditSocialWindow(EditSocialViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.SubmitMessage += SubmitMessageHandler;
            _viewModel.CheckValidationMessage += CheckValidationMessageHandler;
            _viewModel.CancelMessage += CancelMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);

            InitializeComponent();
        }

        private void SubmitMessageHandler(object sender, EventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CheckValidationMessageHandler(object sender, EventArgs e)
        {
            appNameField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            appIdField.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            bool isAppNameValid = !appNameField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isAppIdValid = !appIdField.GetBindingExpression(TextBox.TextProperty).HasError;

            _viewModel.IsModelValid = isAppNameValid && isAppIdValid;
        }

        private void CancelMessageHandler(object sender, EventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.SubmitMessage -= SubmitMessageHandler;
            _viewModel.CheckValidationMessage -= CheckValidationMessageHandler;
            _viewModel.CancelMessage -= CancelMessageHandler;

            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}