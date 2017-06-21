using Organizer.UI.Properties;
using System.Configuration;
using System.Windows;
using Autofac;
using Organizer.DI;
using Organizer.Common.Entities;

namespace Organizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Containter { get; private set; }

        public static string ConnectionString { get; private set; }

        public static User CurrentUser { get; set; }

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
        }

        private static void SetupContainer()
        {
            var containerBuilder = new ContainerBuilder();
            AutofacInitializer.Initialize(containerBuilder, ConnectionString);

            Containter = containerBuilder.Build();
        }
    }
}