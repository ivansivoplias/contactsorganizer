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
            _viewModel.EditContactMessage += EditContactMessageHandler;
            _viewModel.ViewContactMessage += ViewContactMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();
        }

        private void EditContactMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var editContactViewModel = new EditContactViewModel(_viewModel.SelectedContact);

                var addContactWindow = new EditContactWindow(editContactViewModel);

                addContactWindow.Show();

                this.Close();
            });
        }

        private void ViewContactMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewContactViewModel = new ContactDetailsViewModel(_viewModel.SelectedContact);

                var addContactWindow = new ViewContactWindow(viewContactViewModel);

                addContactWindow.Show();

                this.Close();
            });
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
            _viewModel.EditContactMessage -= EditContactMessageHandler;
            _viewModel.ViewContactMessage -= ViewContactMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}