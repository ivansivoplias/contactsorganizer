using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Entities;
using Organizer.Common.Enums;
using Organizer.Common.Exceptions;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using Organizer.UI.Helpers;
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
    public class AddMeetingViewModel : ViewModelBase
    {
        private Command _saveCommand;
        private Command _cancelCommand;
        private Meeting _meeting;
        private ObservableCollection<string> _timeIntervals;
        private ObservableCollection<string> _meetingTypes;
        private IMeetingService _meetingService;

        public event EventHandler SaveMessage = delegate { };

        public event EventHandler CancelMessage = delegate { };

        public event EventHandler CheckValidationMessage = delegate { };

        public ICommand SaveCommand => _saveCommand;

        public ICommand CancelCommand => _cancelCommand;

        public bool IsModelValid { get; set; }

        public bool IsOneTimeMeeting => _meeting.MeetingType == MeetingType.OneTime;

        public string HeaderText => "Add meeting";

        public string MeetingName
        {
            get { return _meeting.MeetingName; }
            set
            {
                _meeting.MeetingName = value;
                OnPropertyChanged(nameof(MeetingName));
            }
        }

        public ICollection<string> TimeIntervals => _timeIntervals;

        public ICollection<string> MeetingTypes => _meetingTypes;

        public string Description
        {
            get { return _meeting.Description; }
            set
            {
                _meeting.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime MeetingDate
        {
            get { return _meeting.MeetingDate; }
            set
            {
                _meeting.MeetingDate = value;
                OnPropertyChanged(nameof(MeetingDate));
            }
        }

        public DateTime NotificationDate
        {
            get { return _meeting.NotificationDate; }
            set
            {
                _meeting.NotificationDate = value;
                OnPropertyChanged(nameof(NotificationDate));
            }
        }

        public string MeetingPlace
        {
            get { return _meeting.MeetingPlace; }
            set
            {
                _meeting.MeetingPlace = value;
                OnPropertyChanged(nameof(MeetingPlace));
            }
        }

        public string MeetingTime
        {
            get { return _meeting.MeetingTime.ToString(@"hh\:mm"); }
            set
            {
                _meeting.MeetingTime = TimeSpan.ParseExact(value, @"hh\:mm", null);
                OnPropertyChanged(nameof(MeetingTime));
            }
        }

        public string NotificationTime
        {
            get { return _meeting.NotificationTime.ToString(@"hh\:mm"); }
            set
            {
                _meeting.NotificationTime = TimeSpan.ParseExact(value, @"hh\:mm", null);
                OnPropertyChanged(nameof(NotificationTime));
            }
        }

        public string TypeOfMeeting
        {
            get { return _meeting.MeetingType.ToString(); }
            set
            {
                _meeting.MeetingType = (MeetingType)Enum.Parse(typeof(MeetingType), value);
                OnPropertyChanged(nameof(TypeOfMeeting));
                OnPropertyChanged(nameof(IsOneTimeMeeting));
            }
        }

        public bool SendNotifications
        {
            get { return _meeting.SendNotifications; }
            set
            {
                _meeting.SendNotifications = value;
                OnPropertyChanged(nameof(SendNotifications));
            }
        }

        public AddMeetingViewModel()
        {
            _meeting = new Meeting()
            {
                UserId = App.CurrentUser.Id,
                MeetingType = MeetingType.OneTime,
                MeetingDate = DateTime.Now,
                NotificationDate = DateTime.Now
            };

            var timeIntervals = TimeIntervalHelper.GetTimeIntervals().Select(x => x.ToString(@"hh\:mm")).ToList();

            var meetingTypes = (Enum.GetValues(typeof(MeetingType)) as MeetingType[])
                .Where(x => x != default(MeetingType))
                .Select(x => x.ToString()).ToList();

            _meetingTypes = new ObservableCollection<string>(meetingTypes);

            _timeIntervals = new ObservableCollection<string>(timeIntervals);

            _meetingService = App.Containter.Resolve<IMeetingService>();

            _saveCommand = Command.CreateCommand("Save meeting", "SaveCommand", GetType(), Save, SaveCanExecute);
            _cancelCommand = Command.CreateCommand("Cancel", "CancelCommand", GetType(), Cancel);
        }

        private void Save()
        {
            CheckValidation();

            if (IsModelValid)
            {
                _meeting.UserId = App.CurrentUser.Id;

                try
                {
                    _meetingService.AddMeeting(_meeting);
                    SaveMessage.Invoke(null, EventArgs.Empty);
                }
                catch (MeetingNameAlreadyExistsException e)
                {
                    MessageBox.Show($"Invalid data provided. Meeting cannot be saved.\nDetails: {e.Message} ", "Add meeting failed!");
                }
                catch
                {
                    MessageBox.Show("Invalid data provided. Meeting cannot be saved.", "Error");
                }
            }
        }

        private bool SaveCanExecute()
        {
            CheckValidation();
            return IsModelValid;
        }

        private void CheckValidation()
        {
            CheckValidationMessage.Invoke(null, EventArgs.Empty);
        }

        private void Cancel()
        {
            CancelMessage.Invoke(null, EventArgs.Empty);
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _saveCommand);
            Command.RegisterCommandBinding(window, _cancelCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _saveCommand);
            Command.UnregisterCommandBinding(window, _cancelCommand);
        }
    }
}