using Organizer.UI.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ContactsListWindow.xaml
    /// </summary>
    public partial class ContactsListWindow : Window
    {
        private ContactsListViewModel _viewModel;

        public ContactsListWindow(ContactsListViewModel viewModel)
        {
            _viewModel = viewModel;

            this.DataContext = _viewModel;
            InitializeComponent();
        }
    }
}