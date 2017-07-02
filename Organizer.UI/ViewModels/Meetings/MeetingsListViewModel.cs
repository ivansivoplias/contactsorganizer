using Autofac;
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
    public class MeetingsListViewModel : ViewModelBase
    {
        private int _pageNumber;
        private const int _numberOnPage = 10;
        private IMeetingService _meetingService;

        private MeetingSearchType _currentSearchType;
        private string _searchValue;

        private ObservableCollection<MeetingDto> _meetings;
        private MeetingDto _selected;
        private Command _searchCommand;
        private Command _addMeetingCommand;
        private Command _deleteMeetingCommand;
        private Command _editMeetingCommand;
        private Command _viewMeetingCommand;
        private Command _backCommand;
        private Command _fetchNextPageCommand;

        public MeetingSearchType SearchType
        {
            get { return _currentSearchType; }
            set
            {
                if (_currentSearchType == value)
                    return;
                _currentSearchType = value;
                OnPropertyChanged(nameof(SearchType));
                //SearchTypeChanged.Invoke(null, EventArgs.Empty);
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
        public ICommand FetchNextPageCommand => _fetchNextPageCommand;

        public ICollection<MeetingDto> Meetings => _meetings;

        public MeetingDto SelectedMeeting
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

            _meetingService = App.Containter.Resolve<IMeetingService>();

            var meetingsList = _meetingService.GetUserMeetings(App.CurrentUser, _numberOnPage, _pageNumber).ToList();

            _meetings = new ObservableCollection<MeetingDto>(meetingsList);

            _addMeetingCommand = Command.CreateCommand("Add meeting", "AddMeeting", GetType(), AddMeeting);
            _deleteMeetingCommand = Command.CreateCommand("Delete meeting", "DeleteMeeting", GetType(),
                DeleteMeeting, () => _selected != null);

            _editMeetingCommand = Command.CreateCommand("Edit meeting", "EditMeeting", GetType(),
                EditMeeting, () => _selected != null);

            _searchCommand = Command.CreateCommand("Search", "Search", GetType(),
                Search);

            _viewMeetingCommand = Command.CreateCommand("View meeting details", "ViewMeeting", GetType(),
                ViewMeetingDetails, () => _selected != null);

            _fetchNextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage);
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
                    _meetingService.RemoveMeeting(_selected);

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
                _pageNumber = 1;
                var list = FetchMeetings(_pageNumber, _numberOnPage);
                _meetings.Clear();
                _meetings = null;

                _meetings = new ObservableCollection<MeetingDto>(list);

                OnPropertyChanged(nameof(Meetings));
            }
        }

        private void RefreshList()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                var list = FetchMeetings(1, _numberOnPage * _pageNumber);
                _meetings.Clear();
                _meetings = null;

                _meetings = new ObservableCollection<MeetingDto>(list);

                OnPropertyChanged(nameof(Meetings));
            }
        }

        private void CheckSearchValidation()
        {
            ValidateSearch.Invoke(null, EventArgs.Empty);
        }

        private void FetchNextPage()
        {
            CheckSearchValidation();
            if (IsSearchValueValid)
            {
                _pageNumber++;
                var meetings = FetchMeetings(_pageNumber, _numberOnPage);
                if (!meetings.IsNullOrEmpty())
                {
                    meetings = _meetings.Union(meetings).ToList();

                    _meetings.Clear();
                    _meetings = null;

                    _meetings = new ObservableCollection<MeetingDto>(meetings);

                    OnPropertyChanged(nameof(Meetings));
                }
                else
                {
                    _pageNumber--;
                }
            }
        }

        private List<MeetingDto> FetchMeetings(int page, int pageSize)
        {
            List<MeetingDto> result;

            switch (_currentSearchType)
            {
                case MeetingSearchType.ByMeetingName:
                    result = _meetingService.FilterByMeetingName(App.CurrentUser, _searchValue, pageSize, page)
                        .ToList();
                    break;

                case MeetingSearchType.ByMeetingDate:
                    var date = DateTime.Parse(_searchValue);
                    result = _meetingService.FilterByMeetingDate(App.CurrentUser, date, pageSize, page)
                        .ToList();
                    break;

                default:
                    result = _meetingService
                        .GetUserMeetings(App.CurrentUser, pageSize, page).ToList();
                    break;
            }

            return result;
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _searchCommand);
            Command.RegisterCommandBinding(window, _addMeetingCommand);
            Command.RegisterCommandBinding(window, _deleteMeetingCommand);
            Command.RegisterCommandBinding(window, _editMeetingCommand);
            Command.RegisterCommandBinding(window, _viewMeetingCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _fetchNextPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _searchCommand);
            Command.UnregisterCommandBinding(window, _addMeetingCommand);
            Command.UnregisterCommandBinding(window, _deleteMeetingCommand);
            Command.UnregisterCommandBinding(window, _editMeetingCommand);
            Command.UnregisterCommandBinding(window, _viewMeetingCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _fetchNextPageCommand);
        }
    }
}