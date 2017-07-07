using Organizer.UI.ViewModels;
using System.Windows;
using System.ComponentModel;
using System;
using System.Windows.Controls;
using Organizer.Common.Enums.SearchTypes;

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
            _viewModel.UpdateViewValidationMessage += SearchTypeChangedHandler;
            _viewModel.ValidateSearch += ValidateSearchHandler;
            _viewModel.BackMessage += BackMessageHandler;
            _viewModel.EditContactMessage += EditContactMessageHandler;
            _viewModel.ViewContactMessage += ViewContactMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();

            this.Title = _viewModel.HeaderText;
        }

        private void EditContactMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var editContactViewModel = new EditContactViewModel(_viewModel.SelectedContact);

                var editContactWindow = new EditContactWindow(editContactViewModel);

                editContactWindow.ShowInTaskbar = false;

                editContactWindow.ShowDialog();
            });
        }

        private void ViewContactMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewContactViewModel = new ContactDetailsViewModel(_viewModel.SelectedContact);

                var viewContactWindow = new ViewContactWindow(viewContactViewModel);

                viewContactWindow.ShowInTaskbar = false;

                viewContactWindow.ShowDialog();
            });
        }

        private void BackMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var startupViewModel = new StartupViewModel();

                var startupWindow = new StartupWindow(startupViewModel);

                startupWindow.Show();

                this.Close();
            });
        }

        private void ValidateSearchHandler(object sender, EventArgs e)
        {
            bool searchValid = !searchBox.GetBindingExpression(TextBox.TextProperty).HasError;
            bool searchNotNull = _viewModel.SearchType != ContactSearchType.Default ?
                !string.IsNullOrEmpty(_viewModel.SearchValue) : true;

            _viewModel.IsSearchValueValid = searchValid && searchNotNull;
        }

        private void SearchTypeChangedHandler(object sender, EventArgs e)
        {
            searchBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void AddContactMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var addContactViewModel = new AddContactViewModel();

                var addContactWindow = new AddContactWindow(addContactViewModel);

                addContactWindow.ShowInTaskbar = false;
                addContactWindow.ShowDialog();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.BackMessage -= BackMessageHandler;
            _viewModel.UpdateViewValidationMessage -= SearchTypeChangedHandler;
            _viewModel.ValidateSearch -= ValidateSearchHandler;
            _viewModel.AddContactMessage -= AddContactMessageHandler;
            _viewModel.EditContactMessage -= EditContactMessageHandler;
            _viewModel.ViewContactMessage -= ViewContactMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }

        private void DataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            bool isBottom = IsScrollViewReachedTheBottom(e);
            if (isBottom)
            {
                _viewModel.NextPageCommand.Execute(null);
            }
        }

        private bool IsScrollViewReachedTheBottom(ScrollChangedEventArgs e)
        {
            if (e.ExtentHeight - e.ViewportHeight == 0 && e.VerticalOffset != 0)
                return true;
            if (e.VerticalOffset == 0)
                return false;
            if (e.ExtentHeight - e.ViewportHeight - e.VerticalOffset == 0)
                return true;
            return false;
        }
    }
}