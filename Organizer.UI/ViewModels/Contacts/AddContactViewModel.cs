using Autofac;
using Organizer.Common.DTO;
using Organizer.Common.Entities;
using Organizer.Common.Exceptions;
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
    public class AddContactViewModel : ViewModelBase
    {
        private Command _saveCommand;
        private Command _addSocialCommand;
        private Command _editSocialCommand;
        private Command _removeSocialCommand;
        private Command _cancelCommand;
        private IContactService _contactService;
        private ContactDto _contact;
        private PersonalInfo _personalInfo;
        private ObservableCollection<SocialInfo> _socials;
        private SocialInfo _selectedSocial;

        public event EventHandler AddSocialMessage = delegate { };

        public event EventHandler EditSocialMessage = delegate { };

        public event EventHandler CheckValidationMessage = delegate { };

        public event EventHandler SaveMessage = delegate { };

        public event EventHandler CancelMessage = delegate { };

        public ICommand AddSocialCommand => _addSocialCommand;

        public ICommand EditSocialCommand => _editSocialCommand;

        public ICommand RemoveSocialCommand => _removeSocialCommand;

        public ICommand SaveCommand => _saveCommand;

        public ICommand CancelCommand => _cancelCommand;

        public ICollection<SocialInfo> Socials => _socials;

        public bool IsModelValid { get; set; }

        public SocialInfo SelectedSocial
        {
            get { return _selectedSocial; }
            set
            {
                _selectedSocial = value;
                OnPropertyChanged(nameof(SelectedSocial));
            }
        }

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

        public string Email
        {
            get { return _personalInfo.Email; }
            set
            {
                _personalInfo.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public AddContactViewModel()
        {
            _contact = new ContactDto();

            _personalInfo = new PersonalInfo();

            _socials = new ObservableCollection<SocialInfo>();

            _contactService = App.Containter.Resolve<IContactService>();

            _saveCommand = Command.CreateCommand("Save contact", "SubmitContact", GetType(), Save);

            _addSocialCommand = Command.CreateCommand("Add social", "AddSocial", GetType(), AddSocial);
            _editSocialCommand = Command.CreateCommand("Edit social", "EditSocial", GetType(), EditSocial, () => _selectedSocial != null);
            _removeSocialCommand = Command.CreateCommand("Remove social", "RemoveSocial", GetType(), RemoveSocial, () => _selectedSocial != null);

            _cancelCommand = Command.CreateCommand("Cancel", "CancelCommand", GetType(), Cancel);
        }

        private void Save()
        {
            CheckValidation();

            if (IsModelValid)
            {
                _contact.UserId = App.CurrentUser.Id;
                _contact.PersonalInfo = _personalInfo;
                _contact.Socials = _socials.ToList();

                try
                {
                    _contactService.AddContact(_contact);
                    SaveMessage.Invoke(null, EventArgs.Empty);
                }
                catch (PrimaryPhoneAlreadyExistException e)
                {
                    MessageBox.Show($"Invalid data provided. Contact cannot be saved.\nDetails:{e.Message}", "Error");
                }
                catch
                {
                    MessageBox.Show("Invalid data provided. Contact cannot be saved.", "Error");
                }
            }
        }

        private void Cancel()
        {
            CancelMessage.Invoke(null, EventArgs.Empty);
        }

        private void CheckValidation()
        {
            CheckValidationMessage.Invoke(null, EventArgs.Empty);
        }

        private void AddSocial()
        {
            AddSocialMessage.Invoke(null, EventArgs.Empty);
        }

        private void EditSocial()
        {
            EditSocialMessage.Invoke(null, EventArgs.Empty);
        }

        private void RemoveSocial()
        {
            var res = MessageBox.Show("Are you sure that you want to delete selected social?", "Social removing confirmation",
                MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                _socials.Remove(_selectedSocial);
            }
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _saveCommand);
            Command.RegisterCommandBinding(window, _addSocialCommand);
            Command.RegisterCommandBinding(window, _removeSocialCommand);
            Command.RegisterCommandBinding(window, _editSocialCommand);
            Command.RegisterCommandBinding(window, _cancelCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _saveCommand);
            Command.UnregisterCommandBinding(window, _addSocialCommand);
            Command.UnregisterCommandBinding(window, _removeSocialCommand);
            Command.UnregisterCommandBinding(window, _editSocialCommand);
            Command.UnregisterCommandBinding(window, _cancelCommand);
        }
    }
}