using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Exceptions;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using Organizer.UI.Helpers;
using System;
using System.Diagnostics;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private string _login;
        private SecureString _password;
        private SecureString _repeatedPassword;

        private IUserService _service;

        private Command _registerCommand;
        private Command _backCommand;

        public ICommand RegisterCommand => _registerCommand;
        public ICommand BackCommand => _backCommand;

        public event EventHandler RegistrationSuccessfulMessage = delegate { };

        public event EventHandler RegistrationFailedMessage = delegate { };

        public event EventHandler CheckValidationMessage = delegate { };

        public event EventHandler BackMessage = delegate { };

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public SecureString RepeatedPassword
        {
            get { return _repeatedPassword; }
            set
            {
                _repeatedPassword = value;
                OnPropertyChanged(nameof(RepeatedPassword));
            }
        }

        public bool IsModelValid { get; set; }

        public string LoginText => "Login";

        public string PasswordText => "Password";

        public string RepeatePasswordText => "Repeate password";

        public string HeaderText => "Register page";

        public RegistrationViewModel()
        {
            _service = App.Containter.Resolve<IUserService>();

            _registerCommand = Command.CreateCommand("Register", "Register", GetType(), Register);
            _backCommand = Command.CreateCommand("Back to login page", "BackCommand", GetType(), Back);
        }

        private void Register()
        {
            CheckValidationMessage.Invoke(null, EventArgs.Empty);

            if (IsModelValid)
            {
                try
                {
                    App.CurrentUser = _service.Register(new UserDto()
                    {
                        Login = Login,
                        Password = Password.SecureStringToString()
                    });
                    SaveUserInSettings();
                    RegistrationSuccessfulMessage.Invoke(null, EventArgs.Empty);
                }
                catch (UserAlreadyExistsException e)
                {
                    MessageBox.Show($"Registration failed. See details below. \nDetails: {e.Message}", "Error! Login failed!");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    RegistrationFailedMessage.Invoke(null, EventArgs.Empty);
                }
            }
        }

        private void Back()
        {
            BackMessage.Invoke(null, EventArgs.Empty);
        }

        private void SaveUserInSettings()
        {
            var settings = Properties.Settings.Default;

            settings.IsUserLoggedIn = true;
            settings.UserLogin = App.CurrentUser.Login;
            settings.UserPassword = App.CurrentUser.Password;

            settings.Save();
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _registerCommand);
            Command.RegisterCommandBinding(window, _backCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _registerCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
        }
    }
}