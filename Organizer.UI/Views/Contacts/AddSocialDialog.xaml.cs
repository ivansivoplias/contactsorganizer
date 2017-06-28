using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for AddSocialDialog.xaml
    /// </summary>
    public partial class AddSocialDialog : Window
    {
        private AddSocialViewModel _viewModel;

        public AddSocialDialog(AddSocialViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.SubmitMessage += SubmitMessageHandler;
            _viewModel.CancelMessage += CancelMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);

            InitializeComponent();
        }

        private void SubmitMessageHandler(object sender, EventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelMessageHandler(object sender, EventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.SubmitMessage -= SubmitMessageHandler;
            _viewModel.CancelMessage -= CancelMessageHandler;

            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}