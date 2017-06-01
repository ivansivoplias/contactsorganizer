using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.DAL.Repository
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private static Lazy<ContactRepository> _contactRepository;
        private static Lazy<MeetingRepository> _meetingRepository;
        private static Lazy<NoteRepository> _noteRepository;
        private static Lazy<PersonalInfoRepository> _personalInfoRepository;
        private static Lazy<UserRepository> _userRepository;

        static RepositoryProvider()
        {
            _contactRepository = new Lazy<ContactRepository>(() => new ContactRepository(), true);
            _meetingRepository = new Lazy<MeetingRepository>(() => new MeetingRepository(), true);
            _noteRepository = new Lazy<NoteRepository>(() => new NoteRepository(), true);
            _personalInfoRepository = new Lazy<PersonalInfoRepository>(() => new PersonalInfoRepository(), true);
            _userRepository = new Lazy<UserRepository>(() => new UserRepository(), true);
        }

        public IRepository<TEntity> GetRepositoryForKey<TEntity>(string typeKey) where TEntity : IEntity
        {
            IRepository<TEntity> result = null;

            if (typeof(IRepository<User>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)_userRepository.Value;
            }
            else if (typeof(IRepository<Contact>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)_contactRepository.Value;
            }
            else if (typeof(IRepository<Meeting>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)_meetingRepository.Value;
            }
            else if (typeof(IRepository<Note>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)_noteRepository.Value;
            }
            else if (typeof(IRepository<PersonalInfo>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)_personalInfoRepository.Value;
            }

            return result;
        }
    }
}