using Autofac;
using AutoMapper;
using GameStore.Common.Hasher;
using Organizer.Common.DTO;
using Organizer.Common.Entities;
using Organizer.DAL.Repository;
using Organizer.Infrastructure.Database;
using Organizer.Infrastructure.Services;
using System;

namespace Organizer.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IContainer _container;

        public UserService(IContainer container)
        {
            _container = container;
        }

        public UserDto Login(string userName, string password, bool isHashed = false)
        {
            UserDto user = null;
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
                        user = Mapper.Map<UserDto>(dbUser);
                    }
                    else
                    {
                        throw new Exception($"Password {password} is incorrect for user {userName}");
                    }
                }
                else
                {
                    throw new Exception($"User with login {userName} are not exists in db.");
                }
            }
            return user;
        }

        public UserDto Register(UserDto newUser)
        {
            UserDto result = null;
            var user = Mapper.Map<User>(newUser);
            var hasher = Sha512Hasher.GetInstance();
            user.Password = hasher.ComputeHash(newUser.Password, null);

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var userRepository = new UserRepository(unitOfWork);

                var dbUser = userRepository.FindByLogin(user.Login);

                if (dbUser == null)
                {
                    using (var transaction = unitOfWork.BeginTransaction())
                    {
                        userRepository.Insert(user, transaction);
                        unitOfWork.Commit();
                    }

                    result = Mapper.Map<UserDto>(userRepository.FindByLogin(user.Login));
                }
                else
                {
                    throw new Exception($"User with login {user.Login} are already exist.");
                }
            }

            return result;
        }
    }
}