using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for EditContactWindow.xaml
    /// </summary>
    public partial class EditContactWindow : Window
    {
        private EditContactViewModel _viewModel;

        public EditContactWindow(EditContactViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.AddSocialMessage += AddSocialMessageHandler;
            _viewModel.EditSocialMessage += EditSocialMessageHandler;
            _viewModel.CancelMessage += CancelMessageHandler;
            _viewModel.SaveMessage += SaveMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);

            InitializeComponent();
        }

        private void SaveMessageHandler(object sender, EventArgs e)
        {
            SetupContactsListForm();
        }

        private void CancelMessageHandler(object sender, EventArgs e)
        {
            SetupContactsListForm();
        }

        private void SetupContactsListForm()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var viewModel = new ContactsListViewModel();
                var contactsList = new ContactsListWindow(viewModel);
                contactsList.Show();
                this.Close();
            });
        }

        private void AddSocialMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var socialViewModel = new AddSocialViewModel(_viewModel.Socials);
                var wnd = new AddSocialDialog(socialViewModel);
                wnd.ShowDialog();
            });
        }

        private void EditSocialMessageHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var socialViewModel = new EditSocialViewModel(_viewModel.Socials, _viewModel.SelectedSocial);
                var wnd = new EditSocialWindow(socialViewModel);
                wnd.ShowDialog();
            });
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.AddSocialMessage -= AddSocialMessageHandler;
            _viewModel.EditSocialMessage -= EditSocialMessageHandler;
            _viewModel.CancelMessage -= CancelMessageHandler;
            _viewModel.SaveMessage -= SaveMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}