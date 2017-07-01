using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for ViewContactWindow.xaml
    /// </summary>
    public partial class ViewContactWindow : Window
    {
        private ContactDetailsViewModel _viewModel;

        public ViewContactWindow(ContactDetailsViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.BackMessage += BackMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);
            InitializeComponent();
        }

        private void BackMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.BackMessage -= BackMessageHandler;
            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}