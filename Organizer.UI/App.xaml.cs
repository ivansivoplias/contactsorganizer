using Organizer.DAL;
using Organizer.DAL.Context;
using Organizer.DAL.Entities;
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

            var factory = new ConnectionFactory(connectionString);
            using (var context = new DbContext(factory.MakeConnection()))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var repo = unitOfWork.GetRepository<User>();
                }
            }
        }
    }
}