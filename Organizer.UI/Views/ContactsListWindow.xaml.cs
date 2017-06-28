using Organizer.UI.ViewModels;
using System.Windows;
using System.ComponentModel;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for ContactsListWindow.xaml
    /// </summary>
    public partial class ContactsListWindow : Window
    {
        private ContactsListViewModel _viewModel;

        public ContactsListWindow(ContactsListViewModel viewModel)
        {
            _viewModel = viewModel;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}