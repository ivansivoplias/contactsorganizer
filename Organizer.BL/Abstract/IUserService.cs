using Organizer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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