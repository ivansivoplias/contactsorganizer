using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Entities;
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
    public class MeetingsListViewModel : ViewModelBase
    {
        private int _pageNumber;
        private const int _numberOnPage = 10;
        private int _totalCount;
        private IMeetingService _meetingservice;

        private MeetingSearchType _currentSearchType;
        private string _searchValue;

        private ObservableCollection<Meeting> _meetings;
        private Meeting _selected;
        private Command _searchCommand;
        private Command _addMeetingCommand;
        private Command _deleteMeetingCommand;
        private Command _editMeetingCommand;
        private Command _viewMeetingCommand;
        private Command _backCommand;

        private Command _nextPageCommand;
        private Command _previousPageCommand;
        private Command _firstPageCommand;
        private Command _lastPageCommand;

        public MeetingSearchType SearchType
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

        public event EventHandler AddMeetingMessage = delegate { };

        public event EventHandler UpdateViewValidationMessage = delegate { };

        public event EventHandler BackMessage = delegate { };

        public event EventHandler DeleteMeetingMessage = delegate { };

        public event EventHandler EditMeetingMessage = delegate { };

        public event EventHandler ViewMeetingMessage = delegate { };

        public event EventHandler ValidateSearch = delegate { };

        public ICommand SearchCommand => _searchCommand;
        public ICommand AddMeetingCommand => _addMeetingCommand;
        public ICommand DeleteMeetingCommand => _deleteMeetingCommand;
        public ICommand EditMeetingCommand => _editMeetingCommand;
        public ICommand ViewMeetingCommand => _viewMeetingCommand;
        public ICommand BackCommand => _backCommand;

        public ICommand NextPageCommand => _nextPageCommand;
        public ICommand PreviousPageCommand => _previousPageCommand;
        public ICommand FirstPageCommand => _firstPageCommand;
        public ICommand LastPageCommand => _lastPageCommand;

        public ICollection<Meeting> Meetings => _meetings;

        public int PagesCount => PaginationHelper.GetPagesCount(_totalCount, _numberOnPage);

        public int CurrentPage => _pageNumber;

        public Meeting SelectedMeeting
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(SelectedMeeting));
            }
        }

        public MeetingsListViewModel()
        {
            _pageNumber = 1;
            _selected = null;
            _currentSearchType = MeetingSearchType.Default;

            _meetingservice = App.Containter.Resolve<IMeetingService>();

            _totalCount = _meetingservice.GetMeetingsCount(App.CurrentUser);

            var meetingsList = _meetingservice.GetUserMeetings(App.CurrentUser, _numberOnPage, _pageNumber).ToList();

            _meetings = new ObservableCollection<Meeting>(meetingsList);

            _addMeetingCommand = Command.CreateCommand("Add meeting", "AddMeeting", GetType(), AddMeeting);
            _deleteMeetingCommand = Command.CreateCommand("Delete meeting", "DeleteMeeting", GetType(),
                DeleteMeeting, () => _selected != null);

            _editMeetingCommand = Command.CreateCommand("Edit meeting", "EditMeeting", GetType(),
                EditMeeting, () => _selected != null);

            _searchCommand = Command.CreateCommand("Search", "Search", GetType(),
                Search);

            _viewMeetingCommand = Command.CreateCommand("View meeting details", "ViewMeeting", GetType(),
                ViewMeetingDetails, () => _selected != null);

            _nextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage, NextPageCanExecuted);
            _previousPageCommand = Command.CreateCommand("Previous page", "PreviousPage", GetType(), FetchPreviousPage, PreviousPageCanExecuted);
            _firstPageCommand = Command.CreateCommand("First page", "FirstPage", GetType(), FetchFirstPage, FirstPageCanExecuted);
            _lastPageCommand = Command.CreateCommand("Last page", "LastPage", GetType(), FetchLastPage, LastPageCanExecuted);
            _backCommand = Command.CreateCommand("Back to main menu", "BackCommand", GetType(), Back);
        }

        private void AddMeeting()
        {
            AddMeetingMessage.Invoke(null, EventArgs.Empty);
            RefreshList();
        }

        private void DeleteMeeting()
        {
            var res = MessageBox.Show("Are you sure that you want to delete this meeting?",
                "Delete meeting confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    _meetingservice.RemoveMeeting(_selected);

                    DeleteMeetingMessage.Invoke(null, EventArgs.Empty);

                    RefreshList();
                }
                catch
                {
                    MessageBox.Show("Delete operation failed. An uncatched error occur\nwhile deleting a meeting.",
                        "Delete failed");
                }
            }
        }

        private void Back()
        {
            BackMessage.Invoke(null, EventArgs.Empty);
        }

        private void EditMeeting()
        {
            EditMeetingMessage.Invoke(null, EventArgs.Empty);
            RefreshList();
        }

        private void ViewMeetingDetails()
        {
            ViewMeetingMessage.Invoke(null, EventArgs.Empty);
        }

        private void Search()
        {
            UpdateViewValidationMessage.Invoke(null, EventArgs.Empty);
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                UpdateTotalCount();
                _pageNumber = 1;
                var list = FetchMeetings(_pageNumber, _numberOnPage);
                _meetings.Clear();
                _meetings = null;

                _meetings = new ObservableCollection<Meeting>(list);

                OnPropertyChanged(nameof(Meetings));
            }
        }

        private void RefreshList()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                UpdateTotalCount();
                var list = FetchMeetings(1, _numberOnPage * _pageNumber);
                _meetings.Clear();
                _meetings = null;

                _meetings = new ObservableCollection<Meeting>(list);

                OnPropertyChanged(nameof(Meetings));
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
                var meetings = FetchMeetings(_pageNumber, _numberOnPage);
                if (!meetings.IsNullOrEmpty())
                {
                    _meetings.Clear();
                    _meetings = null;

                    _meetings = new ObservableCollection<Meeting>(meetings);

                    OnPropertyChanged(nameof(Meetings));
                }
            }
        }

        private void FetchPreviousPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber--;
                var meetings = FetchMeetings(_pageNumber, _numberOnPage);
                if (!meetings.IsNullOrEmpty())
                {
                    _meetings.Clear();
                    _meetings = null;

                    _meetings = new ObservableCollection<Meeting>(meetings);

                    OnPropertyChanged(nameof(Meetings));
                }
            }
        }

        private void FetchFirstPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber = 1;
                var meetings = FetchMeetings(_pageNumber, _numberOnPage);
                if (!meetings.IsNullOrEmpty())
                {
                    _meetings.Clear();
                    _meetings = null;

                    _meetings = new ObservableCollection<Meeting>(meetings);

                    OnPropertyChanged(nameof(Meetings));
                }
            }
        }

        private void FetchLastPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber = PagesCount;
                var meetings = FetchMeetings(_pageNumber, _numberOnPage);
                if (!meetings.IsNullOrEmpty())
                {
                    _meetings.Clear();
                    _meetings = null;

                    _meetings = new ObservableCollection<Meeting>(meetings);

                    OnPropertyChanged(nameof(Meetings));
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
                case MeetingSearchType.ByMeetingName:
                    _totalCount = _meetingservice.GetFilterByMeetingNameCount(App.CurrentUser, _searchValue);
                    break;

                case MeetingSearchType.ByMeetingDate:
                    var date = DateTime.Parse(_searchValue);
                    _totalCount = _meetingservice.GetFilterByMeetingDateCount(App.CurrentUser, date);
                    break;

                default:
                    _totalCount = _meetingservice
                        .GetMeetingsCount(App.CurrentUser);
                    break;
            }
        }

        private List<Meeting> FetchMeetings(int page, int pageSize)
        {
            List<Meeting> result;

            switch (_currentSearchType)
            {
                case MeetingSearchType.ByMeetingName:
                    result = _meetingservice.FilterByMeetingName(App.CurrentUser, _searchValue, pageSize, page)
                        .ToList();
                    break;

                case MeetingSearchType.ByMeetingDate:
                    var date = DateTime.Parse(_searchValue);
                    result = _meetingservice.FilterByMeetingDate(App.CurrentUser, date, pageSize, page)
                        .ToList();
                    break;

                default:
                    result = _meetingservice
                        .GetUserMeetings(App.CurrentUser, pageSize, page).ToList();
                    break;
            }

            return result;
        }

        #endregion Pagination

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _searchCommand);
            Command.RegisterCommandBinding(window, _addMeetingCommand);
            Command.RegisterCommandBinding(window, _deleteMeetingCommand);
            Command.RegisterCommandBinding(window, _editMeetingCommand);
            Command.RegisterCommandBinding(window, _viewMeetingCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _nextPageCommand);
            Command.RegisterCommandBinding(window, _previousPageCommand);
            Command.RegisterCommandBinding(window, _firstPageCommand);
            Command.RegisterCommandBinding(window, _lastPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _searchCommand);
            Command.UnregisterCommandBinding(window, _addMeetingCommand);
            Command.UnregisterCommandBinding(window, _deleteMeetingCommand);
            Command.UnregisterCommandBinding(window, _editMeetingCommand);
            Command.UnregisterCommandBinding(window, _viewMeetingCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _nextPageCommand);
            Command.UnregisterCommandBinding(window, _previousPageCommand);
            Command.UnregisterCommandBinding(window, _firstPageCommand);
            Command.UnregisterCommandBinding(window, _lastPageCommand);
        }
    }
}