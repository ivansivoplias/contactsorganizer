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
            if (Settings.Default.InHomework)
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["HomeDb"].ConnectionString;
            }
            else
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SyvopliasDb"].ConnectionString;
            }

            SetupContainer();

            UIMapperConfigurator.Configure();

            var service = Containter.Resolve<IUserService>();
            var loginViewModel = new LoginViewModel(service);
            var window = new LoginWindow(loginViewModel);
            MainWindow = window;
            MainWindow.Show();
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