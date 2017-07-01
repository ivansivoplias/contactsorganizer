﻿using Organizer.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Organizer.UI.Views
{
    /// <summary>
    /// Interaction logic for AddTodoWindow.xaml
    /// </summary>
    public partial class AddTodoWindow : Window
    {
        private AddTodoViewModel _viewModel;

        public AddTodoWindow(AddTodoViewModel viewModel)
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
            bool isStartDateValid = !startDateField.GetBindingExpression(DatePicker.SelectedDateProperty).HasError;
            bool isEndDateValid = !endDateField.GetBindingExpression(DatePicker.SelectedDateProperty).HasError;

            _viewModel.IsModelValid = isCaptionValid && isTextValid && isStartDateValid && isEndDateValid;
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