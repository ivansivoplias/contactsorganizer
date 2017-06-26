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

            _viewModel.RegistrationFailedMessage += RegistrationFailedMessageHandler;
            _viewModel.RegistrationSuccessfulMessage += RegistrationSucceededMessageHandler;
            _viewModel.LoginFailedMessage += LoginFailedMessageHandler;
            _viewModel.LoginSuccessfulMessage += LoginSucceededMessageHandler;

            this.DataContext = _viewModel;

            this.Closing += OnClosing;

            InitializeComponent();
        }

        private void RegistrationFailedMessageHandler(object sender, EventArgs e)
        {
            MessageBox.Show("Registration failed. User already exists in db.", "Registration failed!");
        }

        private void RegistrationSucceededMessageHandler(object sender, EventArgs e)
        {
            MessageBox.Show("Registration succeeded. User successfully created in db.", "Registration succeeded!");
        }

        private void LoginFailedMessageHandler(object sender, EventArgs e)
        {
            MessageBox.Show("Login failed. User are not exists in db or provided login or password is invalid.", "Login failed!");
        }

        private void LoginSucceededMessageHandler(object sender, EventArgs e)
        {
            MessageBox.Show("Login succeeded. User successfully logged in.", "Login succeeded!");
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Closing -= OnClosing;

            _viewModel.RegistrationFailedMessage -= RegistrationFailedMessageHandler;
            _viewModel.RegistrationSuccessfulMessage -= RegistrationSucceededMessageHandler;
            _viewModel.LoginFailedMessage -= LoginFailedMessageHandler;
            _viewModel.LoginSuccessfulMessage -= LoginSucceededMessageHandler;
            _viewModel?.UnregisterCommandsForWindow(this);
        }

        private void PasswordBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            _viewModel.Password = (sender as PasswordBox).Password;
        }
    }
}