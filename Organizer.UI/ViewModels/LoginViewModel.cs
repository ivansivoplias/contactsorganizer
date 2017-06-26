using Autofac;
using GameStore.Common.Hasher;
using Organizer.Common.DTO;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _login;
        private string _password;
        private Command _loginCommand;
        private Command _registerCommand;
        private Command _exitCommand;
        private IUserService _service;

        public event EventHandler LoginSuccessfulMessage = delegate { };

        public event EventHandler LoginFailedMessage = delegate { };

        public event EventHandler RegistrationSuccessfulMessage = delegate { };

        public event EventHandler RegistrationFailedMessage = delegate { };

        public ICommand LoginCommand => _loginCommand;

        public ICommand RegisterCommand => _registerCommand;

        public string LoginText => "Login";

        public string PasswordText => "Password";

        public string HeaderText => "Login page";

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public LoginViewModel()
        {
            _service = App.Containter.Resolve<IUserService>();

            _loginCommand = Command.CreateCommand("Login", "Login", GetType(), LogIn);
            _registerCommand = Command.CreateCommand("Register", "Register", GetType(), Register);
            _exitCommand = Command.CreateCommand("Exit", "Exit", GetType(), () => Application.Current.Shutdown());
        }

        private void LogIn()
        {
            try
            {
                App.CurrentUser = _service.Login(Login, Password);

                SaveUserInSettings();

                LoginSuccessfulMessage.Invoke(null, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                LoginFailedMessage.Invoke(null, EventArgs.Empty);
            }
        }

        private void Register()
        {
            try
            {
                App.CurrentUser = _service.Register(new UserDto() { Login = Login, Password = Password });
                SaveUserInSettings();
                RegistrationSuccessfulMessage.Invoke(null, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                RegistrationFailedMessage.Invoke(null, EventArgs.Empty);
            }
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
            Command.RegisterCommandBinding(window, _loginCommand);
            Command.RegisterCommandBinding(window, _registerCommand);
            Command.RegisterCommandBinding(window, _exitCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _loginCommand);
            Command.UnregisterCommandBinding(window, _registerCommand);
            Command.UnregisterCommandBinding(window, _exitCommand);
        }
    }
}