using Organizer.UI.Properties;
using System.Configuration;
using System.Windows;

namespace Organizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            ConnectionStringSettings connectionString;
            if (Settings.Default.InHomework)
            {
                connectionString = ConfigurationManager.ConnectionStrings["HomeDb"];
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["SyvopliasDb"];
            }
        }
    }
}