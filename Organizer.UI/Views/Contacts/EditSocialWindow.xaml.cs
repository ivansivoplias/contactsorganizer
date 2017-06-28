using Organizer.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for EditContactWindow.xaml
    /// </summary>
    public partial class EditSocialWindow : Window
    {
        private EditSocialViewModel _viewModel;

        public EditSocialWindow(EditSocialViewModel viewModel)
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