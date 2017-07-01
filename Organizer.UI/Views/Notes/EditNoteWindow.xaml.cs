using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for EditNoteWindow.xaml
    /// </summary>
    public partial class EditNoteWindow : Window
    {
        private EditNoteViewModel _viewModel;

        public EditNoteWindow(EditNoteViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.CancelMessage += CancelMessageHandler;
            _viewModel.CheckValidationMessage += CheckValidationMessageHandler;
            _viewModel.SaveMessage += SaveMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);

            InitializeComponent();
        }

        private void SaveMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckValidationMessageHandler(object sender, EventArgs e)
        {
            bool isCaptionValid = !captionField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isTextValid = !noteTextField.GetBindingExpression(TextBox.TextProperty).HasError;

            _viewModel.IsModelValid = isCaptionValid && isTextValid;
        }

        private void CancelMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.CancelMessage -= CancelMessageHandler;
            _viewModel.CheckValidationMessage -= CheckValidationMessageHandler;
            _viewModel.SaveMessage -= SaveMessageHandler;

            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}