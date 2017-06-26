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
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        private StartupViewModel _viewModel;

        public StartupWindow(StartupViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.RegisterCommandsForWindow(this);

            Closing += OnClosing;

            InitializeComponent();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Closing -= OnClosing;

            _viewModel?.UnregisterCommandsForWindow(this);
        }
    }
}