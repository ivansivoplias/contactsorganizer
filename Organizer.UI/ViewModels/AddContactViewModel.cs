using Autofac;
using Organizer.Infrastructure.Services;
using Organizer.UI.Commands;
using System;
using System.Windows;

namespace Organizer.UI.ViewModels
{
    public class AddContactViewModel : ViewModelBase
    {
        private Command _submitCommand;
        private Command _addSocialCommand;
        private IContactService _contactService;

        public event EventHandler AddSocialMessage = delegate { };

        public AddContactViewModel()
        {
            _contactService = App.Containter.Resolve<IContactService>();

            _submitCommand = Command.CreateCommand("Submit", "SubmitContact", GetType(), Submit);

            _addSocialCommand = Command.CreateCommand("Add social", "AddSocial", GetType(), AddSocial);
        }

        private void Submit()
        {
        }

        private void AddSocial()
        {
            AddSocialMessage.Invoke(null, EventArgs.Empty);
        }

        public override void RegisterCommandsForWindow(Window window)
        {
            Command.RegisterCommandBinding(window, _submitCommand);
            Command.RegisterCommandBinding(window, _addSocialCommand);
        }

        public override void UnregisterCommandsForWindow(Window window)
        {
            Command.UnregisterCommandBinding(window, _submitCommand);
            Command.UnregisterCommandBinding(window, _addSocialCommand);
        }
    }
}