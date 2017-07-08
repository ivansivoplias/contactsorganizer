using Organizer.UI.Helpers;
using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel _viewModel;

        public LoginWindow(LoginViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.RegisterCommandsForWindow(this);

            _viewModel.RegistrationMessage += RegistrationMessageHandler;
            _viewModel.ValidationCheckMessage += ValidationCheckHandler;
            _viewModel.LoginFailedMessage += LoginFailedMessageHandler;
            _viewModel.LoginSuccessfulMessage += LoginSucceededMessageHandler;

            this.DataContext = _viewModel;

            this.Closing += OnClosing;

            InitializeComponent();

            this.Title = _viewModel.HeaderText;
        }

        private void RegistrationMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var registrationViewModel = new RegistrationViewModel();
                var registrationWindow = new RegistrationWindow(registrationViewModel);
                registrationWindow.Show();
                this.Close();
            });
        }

        private void LoginFailedMessageHandler(object sender, EventArgs e)
        {
            MessageBox.Show("Login failed. User are not exists in db or provided login or password is invalid.", "Login failed!");
        }

        private void ValidationCheckHandler(object sender, EventArgs e)
        {
            loginField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            passwordField.GetBindingExpression(PasswordBoxAssistant.BoundPassword).UpdateSource();

            bool isLoginValid = !loginField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isPasswordValid = !passwordField.GetBindingExpression(PasswordBoxAssistant.BoundPassword).HasError;

            _viewModel.IsModelValid = isLoginValid && isPasswordValid;
        }

        private void LoginSucceededMessageHandler(object sender, EventArgs e)
        {
            OpenStartupWindow();
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

            _viewModel.ValidationCheckMessage -= ValidationCheckHandler;
            _viewModel.RegistrationMessage -= RegistrationMessageHandler;
            _viewModel.LoginFailedMessage -= LoginFailedMessageHandler;
            _viewModel.LoginSuccessfulMessage -= LoginSucceededMessageHandler;
            _viewModel?.UnregisterCommandsForWindow(this);
        }
    }
}