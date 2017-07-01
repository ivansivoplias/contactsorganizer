using Organizer.Common.DTO;

namespace Organizer.Infrastructure.Services
{
    public interface IUserService
    {
        UserDto Login(string userName, string password, bool isHashed = false);

        UserDto Register(UserDto newUser);
    }
}