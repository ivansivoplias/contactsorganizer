using Autofac;
using Organizer.Common.DTO;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class ContactsListViewModel : ViewModelBase
    {
        private int _pageNumber;
        private const int _numberOnPage = 10;
        private IContactService _contactService;

        private ObservableCollection<ContactDto> _contacts;
        private ContactDto _selected;
        private Command _addContactCommand;
        private Command _deleteContactCommand;
        private Command _editContactCommand;
        private Command _viewContactCommand;
        private Command _fetchNextPageCommand;

        public event EventHandler AddContactMessage = delegate { };

        public event EventHandler DeleteContactMessage = delegate { };

        public event EventHandler EditContactMessage = delegate { };

        public event EventHandler ViewContactMessage = delegate { };

        public ICommand AddContactCommand => _addContactCommand;
        public ICommand DeleteContactCommand => _deleteContactCommand;
        public ICommand EditContactCommand => _editContactCommand;
        public ICommand ViewContactCommand => _viewContactCommand;
        public ICommand FetchNextPageCommand => _fetchNextPageCommand;

        public ICollection<ContactDto> Contacts => _contacts;

        public ContactDto SelectedContact
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(SelectedContact));
            }
        }

        public ContactsListViewModel()
        {
            _pageNumber = 1;
            _selected = null;

            _contactService = App.Containter.Resolve<IContactService>();

            var contactsList = _contactService.GetContacts(App.CurrentUser, _numberOnPage, _pageNumber)?.ToList();

            _contacts = new ObservableCollection<ContactDto>(contactsList);

            _addContactCommand = Command.CreateCommand("Add contact", "AddContact", GetType(), AddContact);
            _deleteContactCommand = Command.CreateCommand("Delete contact", "DeleteContact", GetType(), DeleteContact, () => _selected != null);
            _editContactCommand = Command.CreateCommand("Edit contact", "EditContact", GetType(), EditContact, () => _selected != null);
            _viewContactCommand = Command.CreateCommand("View contact details", "ViewContact", GetType(), ViewContactDetails, () => _selected != null);
            _fetchNextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage);
        }

        private void AddContact()
        {
            AddContactMessage.Invoke(null, EventArgs.Empty);
        }

        private void DeleteContact()
        {
            DeleteContactMessage.Invoke(null, EventArgs.Empty);
        }

        private void EditContact()
        {
            EditContactMessage.Invoke(null, EventArgs.Empty);
        }

        private void ViewContactDetails()
        {
            ViewContactMessage.Invoke(null, EventArgs.Empty);
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