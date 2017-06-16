using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.BL.Abstract
{
    public interface IUserService
    {
        User Login(string userName, string passWord);

        void Logout(User user);

        User Register(User newUser);

        ICollection<string> GetLatestActivities(User user);
    }
}