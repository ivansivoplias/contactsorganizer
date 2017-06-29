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
    public class MeetingsListViewModel : ViewModelBase
    {
        private int _pageNumber;
        private const int _numberOnPage = 10;
        private IMeetingService _meetingService;

        private ObservableCollection<MeetingDto> _meetings;
        private MeetingDto _selected;
        private Command _addMeetingCommand;
        private Command _deleteMeetingCommand;
        private Command _editMeetingCommand;
        private Command _viewMeetingCommand;
        private Command _backCommand;
        private Command _fetchNextPageCommand;

        public event EventHandler AddMeetingMessage = delegate { };

        public event EventHandler BackMessage = delegate { };

        public event EventHandler DeleteMeetingMessage = delegate { };

        public event EventHandler EditMeetingMessage = delegate { };

        public event EventHandler ViewMeetingMessage = delegate { };

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

            _meetingService = App.Containter.Resolve<IMeetingService>();

            var meetingsList = _meetingService.GetUserMeetings(App.CurrentUser, _numberOnPage, _pageNumber).ToList();

            _meetings = new ObservableCollection<MeetingDto>(meetingsList);

            _addMeetingCommand = Command.CreateCommand("Add meeting", "AddMeeting", GetType(), AddMeeting);
            _deleteMeetingCommand = Command.CreateCommand("Delete meeting", "DeleteMeeting", GetType(),
                DeleteMeeting, () => _selected != null);

            _editMeetingCommand = Command.CreateCommand("Edit meeting", "EditMeeting", GetType(),
                EditMeeting, () => _selected != null);

            _viewMeetingCommand = Command.CreateCommand("View meeting details", "ViewMeeting", GetType(),
                ViewMeetingDetails, () => _selected != null);

            _fetchNextPageCommand = Command.CreateCommand("Next page", "FetchNextPage", GetType(), FetchNextPage);
            _backCommand = Command.CreateCommand("Back to main menu", "BackCommand", GetType(), Back);
        }

        private void AddMeeting()
        {
            AddMeetingMessage.Invoke(null, EventArgs.Empty);
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

                    var meetingsList = _meetingService
                        .GetUserMeetings(App.CurrentUser, _numberOnPage, _pageNumber).ToList();

                    _meetings.Clear();

                    _meetings = null;

                    _meetings = new ObservableCollection<MeetingDto>(meetingsList);

                    OnPropertyChanged(nameof(Meetings));

                    DeleteMeetingMessage.Invoke(null, EventArgs.Empty);
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
        }

        private void ViewMeetingDetails()
        {
            ViewMeetingMessage.Invoke(null, EventArgs.Empty);
        }

        private void FetchNextPage()
        {
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _addMeetingCommand);
            Command.RegisterCommandBinding(window, _deleteMeetingCommand);
            Command.RegisterCommandBinding(window, _editMeetingCommand);
            Command.RegisterCommandBinding(window, _viewMeetingCommand);
            Command.RegisterCommandBinding(window, _backCommand);
            Command.RegisterCommandBinding(window, _fetchNextPageCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _addMeetingCommand);
            Command.UnregisterCommandBinding(window, _deleteMeetingCommand);
            Command.UnregisterCommandBinding(window, _editMeetingCommand);
            Command.UnregisterCommandBinding(window, _viewMeetingCommand);
            Command.UnregisterCommandBinding(window, _backCommand);
            Command.UnregisterCommandBinding(window, _fetchNextPageCommand);
        }
    }
}