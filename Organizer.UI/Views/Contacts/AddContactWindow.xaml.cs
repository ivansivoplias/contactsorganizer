using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        private AddContactViewModel _viewModel;

        public AddContactWindow(AddContactViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.AddSocialMessage += AddSocialMessageHandler;
            _viewModel.CheckValidationMessage += CheckValidationMessageHandler;
            _viewModel.EditSocialMessage += EditSocialMessageHandler;
            _viewModel.CancelMessage += CancelMessageHandler;
            _viewModel.SaveMessage += SaveMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);

            InitializeComponent();
        }

        private void SaveMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CancelMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckValidationMessageHandler(object sender, EventArgs e)
        {
            primaryPhoneField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            firstNameField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            middleNameField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            lastNameField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            nickNameField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            emailField.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            bool isPhoneValid = !primaryPhoneField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isFirstNameValid = !firstNameField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isMiddleNameValid = !middleNameField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isLastNameValid = !lastNameField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isNickNameValid = !nickNameField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isEmailValid = !emailField.GetBindingExpression(TextBox.TextProperty).HasError;

            _viewModel.IsModelValid = isPhoneValid && isFirstNameValid && isMiddleNameValid
                && isLastNameValid && isNickNameValid && isEmailValid;
        }

        private void AddSocialMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var socialViewModel = new AddSocialViewModel(_viewModel.Socials);
                var wnd = new AddSocialDialog(socialViewModel);
                wnd.ShowDialog();
            });
        }

        private void EditSocialMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var socialViewModel = new EditSocialViewModel(_viewModel.Socials, _viewModel.SelectedSocial);
                var wnd = new EditSocialWindow(socialViewModel);
                wnd.ShowDialog();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.CheckValidationMessage -= CheckValidationMessageHandler;
            _viewModel.AddSocialMessage -= AddSocialMessageHandler;
            _viewModel.EditSocialMessage -= EditSocialMessageHandler;
            _viewModel.CancelMessage -= CancelMessageHandler;
            _viewModel.SaveMessage -= SaveMessageHandler;

            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}