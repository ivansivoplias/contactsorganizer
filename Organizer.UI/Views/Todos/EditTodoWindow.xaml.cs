using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for EditTodoWindow.xaml
    /// </summary>
    public partial class EditTodoWindow : Window
    {
        private EditTodoViewModel _viewModel;

        public EditTodoWindow(EditTodoViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.CancelMessage += CancelMessageHandler;
            _viewModel.CheckValidationMessage += CheckValidationMessageHandler;
            _viewModel.SaveMessage += SaveMessageHandler;

            this.DataContext = _viewModel;
            this.Closing += OnClosing;

            _viewModel.RegisterCommandsForWindow(this);

            InitializeComponent();

            this.Title = _viewModel.HeaderText;
        }

        private void SaveMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CancelMessageHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckValidationMessageHandler(object sender, EventArgs e)
        {
            captionField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            noteTextField.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            startDateField.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
            endDateField.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();

            bool isCaptionValid = !captionField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isTextValid = !noteTextField.GetBindingExpression(TextBox.TextProperty).HasError;
            bool isStartDateValid = !startDateField.GetBindingExpression(DatePicker.SelectedDateProperty).HasError;
            bool isEndDateValid = !endDateField.GetBindingExpression(DatePicker.SelectedDateProperty).HasError;

            _viewModel.IsModelValid = isCaptionValid && isTextValid && isStartDateValid && isEndDateValid;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Closing -= OnClosing;

            _viewModel.CheckValidationMessage -= CheckValidationMessageHandler;
            _viewModel.CancelMessage -= CancelMessageHandler;
            _viewModel.SaveMessage -= SaveMessageHandler;

            _viewModel.UnregisterCommandsForWindow(this);
        }
    }
}