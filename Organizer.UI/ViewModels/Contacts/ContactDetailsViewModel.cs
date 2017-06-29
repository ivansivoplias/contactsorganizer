using Organizer.Common.DTO;
using Organizer.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class ContactDetailsViewModel : ViewModelBase
    {
        private ContactDto _contact;
        private ObservableCollection<SocialInfoDto> _socials;
        private Command _backCommand;

        public ICollection<SocialInfoDto> Socials => _socials;

        public ICommand BackCommand => _backCommand;

        public event EventHandler BackMessage = delegate { };

        public string PrimaryPhone => _contact.PrimaryPhone;

        public string FirstName => _contact.PersonalInfo.FirstName;

        public string LastName => _contact.PersonalInfo.Lastname;

        public string MiddleName => _contact.PersonalInfo.MiddleName;

        public string NickName => _contact.PersonalInfo.Nickname;

        public string Email => _contact.PersonalInfo.Email;

        public ContactDetailsViewModel(ContactDto contact)
        {
            _contact = contact;

            _socials = new ObservableCollection<SocialInfoDto>(_contact.Socials);

            _backCommand = Command.CreateCommand("Back to contact list", "BackCommand", GetType(), Back);
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