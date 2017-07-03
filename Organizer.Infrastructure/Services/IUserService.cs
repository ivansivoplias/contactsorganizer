using Organizer.Common.Entities;

namespace Organizer.Infrastructure.Services
{
    public interface IUserService
    {
        User Login(string userName, string password, bool isHashed = false);

        User Register(User newUser);
    }
}