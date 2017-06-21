using System.Collections.Generic;
using Organizer.Common.Entities;

namespace Organizer.Infrastructure.Services
{
    public interface IUserService
    {
        User Login(string userName, string password);

        void Logout(User user);

        User Register(User newUser);

        ICollection<string> GetLatestActivities(User user);
    }
}