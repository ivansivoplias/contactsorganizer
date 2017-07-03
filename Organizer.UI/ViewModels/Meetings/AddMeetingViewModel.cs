using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Entities;
using Organizer.Common.Exceptions;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Collections.Generic;
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
        private IMeetingService _meetingService;

        public event EventHandler SaveMessage = delegate { };

        public event EventHandler CancelMessage = delegate { };

        public event EventHandler CheckValidationMessage = delegate { };

        public ICommand SaveCommand => _saveCommand;

        public ICommand CancelCommand => _cancelCommand;

        public bool IsModelValid { get; set; }

        public string MeetingName
        {
            get { return _meeting.MeetingName; }
            set
            {
                _meeting.MeetingName = value;
                OnPropertyChanged(nameof(MeetingName));
            }
        }

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
                MeetingDate = DateTime.Now,
                NotificationDate = DateTime.Today.AddDays(1)
            };

            _meetingService = App.Containter.Resolve<IMeetingService>();

            _saveCommand = Command.CreateCommand("Save meeting", "SaveCommand", GetType(), Save);
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