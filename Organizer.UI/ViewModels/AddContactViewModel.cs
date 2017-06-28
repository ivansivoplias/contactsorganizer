using Autofac;
using Organizer.Common.DTO;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Windows;

namespace Organizer.UI.ViewModels
{
    public class AddContactViewModel : ViewModelBase
    {
        private Command _saveCommand;
        private Command _addSocialCommand;
        private Command _cancelCommand;
        private IContactService _contactService;
        private ContactDto _contact;
        private PersonalInfoDto _personalInfo;

        public event EventHandler AddSocialMessage = delegate { };

        public event EventHandler CancelMessage = delegate { };

        public string PrimaryPhone
        {
            get { return _contact.PrimaryPhone; }
            set
            {
                _contact.PrimaryPhone = value;
                OnPropertyChanged(nameof(PrimaryPhone));
            }
        }

        public string FirstName
        {
            get { return _personalInfo.FirstName; }
            set
            {
                _personalInfo.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return _personalInfo.Lastname; }
            set
            {
                _personalInfo.Lastname = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string MiddleName
        {
            get { return _personalInfo.MiddleName; }
            set
            {
                _personalInfo.MiddleName = value;
                OnPropertyChanged(nameof(MiddleName));
            }
        }

        public string NickName
        {
            get { return _personalInfo.Nickname; }
            set
            {
                _personalInfo.Nickname = value;
                OnPropertyChanged(nameof(NickName));
            }
        }

        public AddContactViewModel()
        {
            _contactService = App.Containter.Resolve<IContactService>();

            _saveCommand = Command.CreateCommand("Save contact", "SubmitContact", GetType(), Save);

            _addSocialCommand = Command.CreateCommand("Add socials", "AddSocials", GetType(), AddSocials);

            _cancelCommand = Command.CreateCommand("Cancel", "CancelCommand", GetType(), Cancel);
        }

        private void Save()
        {
        }

        private void Cancel()
        {
            CancelMessage.Invoke(null, EventArgs.Empty);
        }

        private void AddSocials()
        {
            AddSocialMessage.Invoke(null, EventArgs.Empty);
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _saveCommand);
            Command.RegisterCommandBinding(window, _addSocialCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _saveCommand);
            Command.UnregisterCommandBinding(window, _addSocialCommand);
        }
    }
}