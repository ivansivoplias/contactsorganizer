using Organizer.UI.Properties;
using System.Configuration;
using System.Windows;
using Autofac;
using Organizer.DI;
using System;
using Organizer.Common.DTO;
using Organizer.UI.MapperConfiguration;
using Organizer.UI.Views;
using Organizer.UI.ViewModels;
using Organizer.Infrastructure.Services;

namespace Organizer.UI
{
    public partial class App : Application
    {
        public static IContainer Containter { get; private set; }

        public static string ConnectionString { get; private set; }

        public static UserDto CurrentUser { get; set; }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var settings = Settings.Default;

            if (settings.InHomework)
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["HomeDb"].ConnectionString;
            }
            else
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SyvopliasDb"].ConnectionString;
            }

            SetupContainer();

            UIMapperConfigurator.Configure();

            if (!settings.IsUserLoggedIn)
            {
                var loginViewModel = new LoginViewModel();
                MainWindow = new LoginWindow(loginViewModel);
                MainWindow.Show();
            }
            else
            {
                var userService = Containter.Resolve<IUserService>();
                CurrentUser = userService.Login(settings.UserLogin, settings.UserPassword, true);
                var startupViewModel = new StartupViewModel();
                MainWindow = new StartupWindow(startupViewModel);
                MainWindow.Show();
            }
        }

        private static void SetupContainer()
        {
            Func<IContainer> factory = () => Containter;
            var containerBuilder = new ContainerBuilder();
            AutofacInitializer.Initialize(containerBuilder, ConnectionString, factory);

            Containter = containerBuilder.Build();
        }
    }
}