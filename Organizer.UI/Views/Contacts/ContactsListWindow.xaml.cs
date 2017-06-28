using Organizer.UI.ViewModels;
using System.Windows;
using System.ComponentModel;
using System;

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
            _viewModel.AddContactMessage += AddContactMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();
        }

        private void AddContactMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var addContactViewModel = new AddContactViewModel();

                var addContactWindow = new AddContactWindow(addContactViewModel);

                addContactWindow.Show();

                this.Close();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.AddContactMessage -= AddContactMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}