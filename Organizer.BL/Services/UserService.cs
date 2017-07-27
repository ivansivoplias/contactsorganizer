using Autofac;
using GameStore.Common.Hasher;
using Organizer.Common.Entities;
using Organizer.Common.Exceptions;
using Organizer.DAL.Repository;
using Organizer.Infrastructure.Database;
using Organizer.Infrastructure.Services;

namespace Organizer.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IContainer _container;

        public UserService(IContainer container)
        {
            _container = container;
        }

        public User Login(string userName, string password, bool isHashed = false)
        {
            User user = null;
            var hasher = Sha512Hasher.GetInstance();
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var userRepository = new UserRepository(unitOfWork);

                var dbUser = userRepository.FindByLogin(userName);
                if (dbUser != null)
                {
                    if (hasher.VerifyHash(password, dbUser.Password) || isHashed && dbUser.Password.Equals(password))
                    {
                        user = dbUser;
                    }
                    else
                    {
                        throw new LoginFailedException($"Password {password} is incorrect for user {userName}");
                    }
                }
                else
                {
                    throw new LoginFailedException($"User with login {userName} are not exists in db.");
                }
            }
            return user;
        }

        public User Register(User newUser)
        {
            User result = null;
            var hasher = Sha512Hasher.GetInstance();
            newUser.Password = hasher.ComputeHash(newUser.Password, null);

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var userRepository = new UserRepository(unitOfWork);

                var dbUser = userRepository.FindByLogin(newUser.Login);

                if (dbUser == null)
                {
                    unitOfWork.BeginTransaction();
                    userRepository.Insert(newUser);
                    unitOfWork.Commit();

                    result = userRepository.FindByLogin(newUser.Login);
                }
                else
                {
                    throw new UserAlreadyExistsException($"User with login {newUser.Login} are already exist.");
                }
            }

            return result;
        }
    }
}