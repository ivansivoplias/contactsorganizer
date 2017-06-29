using Organizer.Common.DTO;
using Organizer.UI.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class MeetingDetailsViewModel : ViewModelBase
    {
        private Command _backCommand;
        private MeetingDto _meeting;

        public event EventHandler BackMessage = delegate { };

        public ICommand BackCommand => _backCommand;

        public string MeetingName => _meeting.MeetingName;

        public string Description => _meeting.Description;

        public DateTime MeetingDate => _meeting.MeetingDate;

        public DateTime NotificationDate => _meeting.NotificationDate;

        public bool SendNotifications => _meeting.SendNotifications;

        public MeetingDetailsViewModel(MeetingDto meeting)
        {
            _meeting = meeting;

            _backCommand = Command.CreateCommand("Back", "BackCommand", GetType(), Back);
        }

        private void Back()
        {
            BackMessage.Invoke(null, EventArgs.Empty);
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _backCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _backCommand);
        }
    }
}