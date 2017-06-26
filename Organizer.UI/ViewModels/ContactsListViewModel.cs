using Organizer.Common.DTO;
using Organizer.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class ContactsListViewModel : ViewModelBase
    {
        private int _pageNumber;
        private const int _numberOnPage = 10;

        private ObservableCollection<ContactDto> _contacts;
        private ContactDto _selected;
        private Command _addContactCommand;
        private Command _deleteContactCommand;
        private Command _editContactCommand;
        private Command _viewContactCommand;
        private Command _fetchNextPageCommand;

        public ICommand AddContactCommand => _addContactCommand;
        public ICommand DeleteContactCommand => _deleteContactCommand;
        public ICommand EditContactCommand => _editContactCommand;
        public ICommand ViewContactCommand => _viewContactCommand;
        public ICommand FetchNextPageCommand => _fetchNextPageCommand;

        public ICollection<ContactDto> Contacts => _contacts;

        public ContactsListViewModel()
        {
            _pageNumber = 1;
            _addContactCommand = Command.CreateCommand("Add contact", "AddContact", GetType(), AddContact);
            _deleteContactCommand = Command.CreateCommand("Delete contact", "DeleteContact", GetType(), DeleteContact);
            _editContactCommand = Command.CreateCommand("Edit contact", "EditContact", GetType(), EditContact);
            _viewContactCommand = Command.CreateCommand("View contact details", "ViewContact", GetType(), ViewContactDetails);
            _fetchNextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage);
        }

        private void AddContact()
        {
        }

        private void DeleteContact()
        {
        }

        private void EditContact()
        {
        }

        private void ViewContactDetails()
        {
        }

        private void FetchNextPage()
        {
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _addContactCommand);
            Command.RegisterCommandBinding(window, _deleteContactCommand);
            Command.RegisterCommandBinding(window, _editContactCommand);
            Command.RegisterCommandBinding(window, _viewContactCommand);
            Command.RegisterCommandBinding(window, _fetchNextPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _addContactCommand);
            Command.UnregisterCommandBinding(window, _deleteContactCommand);
            Command.UnregisterCommandBinding(window, _editContactCommand);
            Command.UnregisterCommandBinding(window, _viewContactCommand);
            Command.UnregisterCommandBinding(window, _fetchNextPageCommand);
        }
    }
}