using Organizer.Common.DTO;
using Organizer.Common.Entities;
using Organizer.UI.Commands;
using Organizer.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.ViewModels
{
    public class EditSocialViewModel : ViewModelBase
    {
        private SocialInfo _socialInfo;
        private SocialInfo _edited;
        private Command _saveCommand;
        private Command _cancelCommand;
        private ObservableCollection<string> _standartSocials;
        private ICollection<SocialInfo> _socials;

        public event EventHandler SubmitMessage = delegate { };

        public event EventHandler CancelMessage = delegate { };

        public event EventHandler CheckValidationMessage = delegate { };

        public ICommand SaveCommand => _saveCommand;

        public ICommand CancelCommand => _cancelCommand;

        public ICollection<string> StandartSocials => _standartSocials;

        public bool IsModelValid { get; set; }

        public SocialInfo Social => _socialInfo;

        public string HeaderText => "Edit social";

        public string AppName
        {
            get { return _socialInfo.AppName; }
            set
            {
                _socialInfo.AppName = value;
                OnPropertyChanged(nameof(AppName));
            }
        }

        public string AppId
        {
            get { return _socialInfo.AppId; }
            set
            {
                _socialInfo.AppId = value;
                OnPropertyChanged(nameof(AppId));
            }
        }

        public EditSocialViewModel(ICollection<SocialInfo> socials, SocialInfo edited)
        {
            _edited = edited;

            _socialInfo = new SocialInfo()
            {
                Id = edited.Id,
                ContactId = edited.ContactId,
                AppName = edited.AppName,
                AppId = edited.AppId
            };

            _socials = socials;

            var predefined = SocialsHelper.GetPredefinedSocials();

            _standartSocials = new ObservableCollection<string>(predefined);

            _saveCommand = Command.CreateCommand("Save", "SaveCommand", GetType(), Save);
            _cancelCommand = Command.CreateCommand("Cancel", "CancelCommand", GetType(), Cancel);
        }

        private void Save()
        {
            CheckValidation();

            if (IsModelValid)
            {
                if (_socials.FirstOrDefault(x => x.AppName == _socialInfo.AppName && x.AppId == _socialInfo.AppId) == null)
                {
                    _socials.Remove(_edited);
                    _socials.Add(_socialInfo);
                    SubmitMessage.Invoke(null, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Cannot save changes to social, because such social already exists!", "Submit failed!", MessageBoxButton.OK);
                }
            }
        }

        private void CheckValidation()
        {
            CheckValidationMessage.Invoke(null, EventArgs.Empty);
        }

        private void Cancel()
        {
            var result = MessageBox.Show("Are you sure to cancel adding?", "Cancel dialog!", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                CancelMessage.Invoke(null, EventArgs.Empty);
            }
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