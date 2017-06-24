using Organizer.Common.DTO;

namespace Organizer.Infrastructure.Services
{
    public interface IUserService
    {
        UserDto Login(string userName, string password);

        UserDto Register(UserDto newUser);
    }
}