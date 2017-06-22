using Organizer.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organizer.Common.Entities;
using Organizer.Infrastructure.Database;

namespace Organizer.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabaseContextFactory _contextFactory;

        public UserService(IDatabaseContextFactory factory)
        {
            _contextFactory = factory;
        }

        public User Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public User Register(User newUser)
        {
            throw new NotImplementedException();
        }
    }
}