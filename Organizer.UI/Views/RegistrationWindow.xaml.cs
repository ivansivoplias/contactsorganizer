using Organizer.UI.Helpers;
using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private RegistrationViewModel _viewModel;

        public RegistrationWindow(RegistrationViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.RegisterCommandsForWindow(this);

            _viewModel.RegistrationFailedMessage += RegistrationMessageHandler;
            _viewModel.RegistrationSuccessfulMessage += RegistrationSuccessfulMessageHandler;
            _viewModel.CheckValidationMessage += CheckValidationMessageHandler;

            this.DataContext = _viewModel;

            this.Closing += OnClosing;

            InitializeComponent();
        }

        private void RegistrationMessageHandler(object sender, EventArgs e)
        {
            MessageBox.Show("Registration failed, such login already exists.", "Registration error");
        }

        private void RegistrationSuccessfulMessageHandler(object sender, EventArgs e)
        {
            OpenStartupWindow();
        }

        private void CheckValidationMessageHandler(object sender, EventArgs e)
        {
            bool loginValid = !loginField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool passwordValid = !passwordField.GetBindingExpression(PasswordBoxAssistant.BoundPassword).HasError;
            bool repeatedPasswordValid = !repeatedPasswordField.GetBindingExpression(PasswordBoxAssistant.BoundPassword).HasError;
            bool loginExists = !string.IsNullOrEmpty(_viewModel.Login);
            bool passExist = !_viewModel.Password.IsNullOrEmpty();
            bool repeatPassExist = !_viewModel.RepeatedPassword.IsNullOrEmpty();

            _viewModel.IsModelValid = loginValid && passwordValid && repeatedPasswordValid
                && loginExists && passExist && repeatPassExist;
        }

        private void OpenStartupWindow()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var startupViewModel = new StartupViewModel();
                var startupWindow = new StartupWindow(startupViewModel);
                startupWindow.Show();
                this.Close();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Closing -= OnClosing;

            _viewModel.RegistrationFailedMessage -= RegistrationMessageHandler;
            _viewModel.RegistrationSuccessfulMessage -= RegistrationSuccessfulMessageHandler;
            _viewModel.CheckValidationMessage -= CheckValidationMessageHandler;
            _viewModel?.UnregisterCommandsForWindow(this);
        }
    }
}