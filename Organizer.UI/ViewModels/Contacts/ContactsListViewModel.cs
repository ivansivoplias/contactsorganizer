﻿using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Enums.SearchTypes;
using Organizer.Common.Helpers;
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
        private int _totalCount;
        private IContactService _contactService;
        private ContactSearchType _currentSearchType;
        private string _searchValue;

        private ObservableCollection<ContactDto> _contacts;
        private ContactDto _selected;
        private Command _searchCommand;
        private Command _addContactCommand;
        private Command _deleteContactCommand;
        private Command _editContactCommand;
        private Command _viewContactCommand;
        private Command _backCommand;

        private Command _nextPageCommand;
        private Command _previousPageCommand;
        private Command _firstPageCommand;
        private Command _lastPageCommand;

        public event EventHandler AddContactMessage = delegate { };

        public event EventHandler UpdateViewValidationMessage = delegate { };

        public event EventHandler BackMessage = delegate { };

        public event EventHandler DeleteContactMessage = delegate { };

        public event EventHandler EditContactMessage = delegate { };

        public event EventHandler ViewContactMessage = delegate { };

        public event EventHandler ValidateSearch = delegate { };

        public ICommand SearchCommand => _searchCommand;
        public ICommand AddContactCommand => _addContactCommand;
        public ICommand DeleteContactCommand => _deleteContactCommand;
        public ICommand EditContactCommand => _editContactCommand;
        public ICommand ViewContactCommand => _viewContactCommand;
        public ICommand BackCommand => _backCommand;
        public ICommand NextPageCommand => _nextPageCommand;
        public ICommand PreviousPageCommand => _previousPageCommand;
        public ICommand FirstPageCommand => _firstPageCommand;
        public ICommand LastPageCommand => _lastPageCommand;

        public int PagesCount => PaginationHelper.GetPagesCount(_totalCount, _numberOnPage);

        public int CurrentPage => _pageNumber;

        public string HeaderText => "Contacts list";

        public ContactSearchType SearchType
        {
            get { return _currentSearchType; }
            set
            {
                if (_currentSearchType == value)
                    return;
                _currentSearchType = value;
                OnPropertyChanged(nameof(SearchType));
            }
        }

        public string SearchValue
        {
            get { return _searchValue; }
            set
            {
                _searchValue = value;
                OnPropertyChanged(nameof(SearchValue));
            }
        }

        public bool IsSearchValueValid { get; set; }

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
            _currentSearchType = ContactSearchType.Default;

            _contactService = App.Containter.Resolve<IContactService>();

            _totalCount = _contactService.GetContactsCount(App.CurrentUser);

            var contactsList = _contactService.GetContacts(App.CurrentUser, _numberOnPage, _pageNumber).ToList();

            _contacts = new ObservableCollection<ContactDto>(contactsList);

            _addContactCommand = Command.CreateCommand("Add contact", "AddContact", GetType(), AddContact);
            _deleteContactCommand = Command.CreateCommand("Delete contact", "DeleteContact", GetType(),
                DeleteContact, () => _selected != null);

            _editContactCommand = Command.CreateCommand("Edit contact", "EditContact", GetType(),
                EditContact, () => _selected != null);

            _searchCommand = Command.CreateCommand("Search", "Search", GetType(),
                Search);

            _viewContactCommand = Command.CreateCommand("View contact details", "ViewContact", GetType(),
                ViewContactDetails, () => _selected != null);

            _nextPageCommand = Command.CreateCommand("Next page", "NextPage", GetType(), FetchNextPage, NextPageCanExecuted);
            _previousPageCommand = Command.CreateCommand("Previous page", "PreviousPage", GetType(), FetchPreviousPage, PreviousPageCanExecuted);
            _firstPageCommand = Command.CreateCommand("First page", "FirstPage", GetType(), FetchFirstPage, FirstPageCanExecuted);
            _lastPageCommand = Command.CreateCommand("Last page", "LastPage", GetType(), FetchLastPage, LastPageCanExecuted);
            _backCommand = Command.CreateCommand("Back to main menu", "BackCommand", GetType(), Back);
        }

        private void AddContact()
        {
            AddContactMessage.Invoke(null, EventArgs.Empty);

            RefreshList();
        }

        private void DeleteContact()
        {
            var res = MessageBox.Show("Are you sure that you want to delete this contact?",
                "Delete contact confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    _contactService.RemoveContact(_selected);

                    DeleteContactMessage.Invoke(null, EventArgs.Empty);

                    RefreshList();
                }
                catch
                {
                    MessageBox.Show("Delete operation failed. An uncatched error occur\nwhile deleting a contact.",
                        "Delete failed");
                }
            }
        }

        private void Back()
        {
            BackMessage.Invoke(null, EventArgs.Empty);
        }

        private void EditContact()
        {
            EditContactMessage.Invoke(null, EventArgs.Empty);
            RefreshList();
        }

        private void ViewContactDetails()
        {
            ViewContactMessage.Invoke(null, EventArgs.Empty);
        }

        private void Search()
        {
            UpdateViewValidationMessage.Invoke(null, EventArgs.Empty);
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                UpdateTotalCount();
                _pageNumber = 1;
                var list = FetchContacts(_pageNumber, _numberOnPage);
                _contacts.Clear();
                _contacts = null;

                _contacts = new ObservableCollection<ContactDto>(list);

                OnPropertyChanged(nameof(Contacts));
            }
        }

        private void RefreshList()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                UpdateTotalCount();
                var list = FetchContacts(_pageNumber, _numberOnPage);
                _contacts.Clear();
                _contacts = null;

                _contacts = new ObservableCollection<ContactDto>(list);

                OnPropertyChanged(nameof(Contacts));
            }
        }

        private void CheckSearchValidation()
        {
            ValidateSearch.Invoke(null, EventArgs.Empty);
        }

        #region Pagination

        private void FetchNextPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber++;
                var contacts = FetchContacts(_pageNumber, _numberOnPage);
                if (!contacts.IsNullOrEmpty())
                {
                    _contacts.Clear();
                    _contacts = null;

                    _contacts = new ObservableCollection<ContactDto>(contacts);

                    OnPropertyChanged(nameof(Contacts));
                }
            }
        }

        private void FetchPreviousPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber--;
                var contacts = FetchContacts(_pageNumber, _numberOnPage);
                if (!contacts.IsNullOrEmpty())
                {
                    _contacts.Clear();
                    _contacts = null;

                    _contacts = new ObservableCollection<ContactDto>(contacts);

                    OnPropertyChanged(nameof(Contacts));
                }
            }
        }

        private void FetchFirstPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber = 1;
                var contacts = FetchContacts(_pageNumber, _numberOnPage);
                if (!contacts.IsNullOrEmpty())
                {
                    _contacts.Clear();
                    _contacts = null;

                    _contacts = new ObservableCollection<ContactDto>(contacts);

                    OnPropertyChanged(nameof(Contacts));
                }
            }
        }

        private void FetchLastPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber = PagesCount;
                var contacts = FetchContacts(_pageNumber, _numberOnPage);
                if (!contacts.IsNullOrEmpty())
                {
                    _contacts.Clear();
                    _contacts = null;

                    _contacts = new ObservableCollection<ContactDto>(contacts);

                    OnPropertyChanged(nameof(Contacts));
                }
            }
        }

        private bool NextPageCanExecuted()
        {
            return _pageNumber + 1 <= PagesCount;
        }

        private bool PreviousPageCanExecuted()
        {
            return _pageNumber > 1;
        }

        private bool FirstPageCanExecuted()
        {
            return _pageNumber != 1 && PagesCount != 0;
        }

        private bool LastPageCanExecuted()
        {
            return _pageNumber != PagesCount && PagesCount != 0;
        }

        private void UpdateTotalCount()
        {
            switch (_currentSearchType)
            {
                case ContactSearchType.ByPhone:
                    _totalCount = _contactService.GetContactsByPhoneCount(App.CurrentUser, _searchValue);
                    break;

                case ContactSearchType.ByPersonalInfo:
                    _totalCount = _contactService.GetContactsByPersonalInfoCount(App.CurrentUser, _searchValue);
                    break;

                case ContactSearchType.BySocialInfo:
                    _totalCount = _contactService.GetContactsBySocialInfoCount(App.CurrentUser, _searchValue);
                    break;

                default:
                    _totalCount = _contactService.GetContactsCount(App.CurrentUser);
                    break;
            }
        }

        private List<ContactDto> FetchContacts(int page, int pageSize)
        {
            List<ContactDto> result;

            switch (_currentSearchType)
            {
                case ContactSearchType.ByPhone:
                    result = _contactService.GetContactsByPhone(App.CurrentUser, _searchValue, pageSize, page)
                        .ToList();
                    break;

                case ContactSearchType.ByPersonalInfo:
                    result = _contactService.GetContactsByPersonalInfo(App.CurrentUser, _searchValue, pageSize, page)
                        .ToList();
                    break;

                case ContactSearchType.BySocialInfo:
                    result = _contactService.GetContacsBySocialInfo(App.CurrentUser, _searchValue, pageSize, page)
                        .ToList();
                    break;

                default:
                    result = _contactService.GetContacts(App.CurrentUser, pageSize, page)
                        .ToList();
                    break;
            }

            return result;
        }

        #endregion Pagination

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _searchCommand);
            Command.RegisterCommandBinding(window, _addContactCommand);
            Command.RegisterCommandBinding(window, _deleteContactCommand);
            Command.RegisterCommandBinding(window, _editContactCommand);
            Command.RegisterCommandBinding(window, _viewContactCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _nextPageCommand);
            Command.RegisterCommandBinding(window, _previousPageCommand);
            Command.RegisterCommandBinding(window, _firstPageCommand);
            Command.RegisterCommandBinding(window, _lastPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _searchCommand);
            Command.UnregisterCommandBinding(window, _addContactCommand);
            Command.UnregisterCommandBinding(window, _deleteContactCommand);
            Command.UnregisterCommandBinding(window, _editContactCommand);
            Command.UnregisterCommandBinding(window, _viewContactCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _nextPageCommand);
            Command.UnregisterCommandBinding(window, _previousPageCommand);
            Command.UnregisterCommandBinding(window, _firstPageCommand);
            Command.UnregisterCommandBinding(window, _lastPageCommand);
        }
    }
}