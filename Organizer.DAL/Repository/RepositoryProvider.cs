using Organizer.DAL.Entities;
using Organizer.Infrastructure;

namespace Organizer.DAL.Repository
{
    public class RepositoryProvider : IRepositoryProvider
    {
        public IRepository<TEntity> GetRepositoryForKey<TEntity>(string typeKey, IDbContext context) where TEntity : IEntity
        {
            IRepository<TEntity> result = null;

            if (typeof(IRepository<User>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)new UserRepository(context);
            }
            else if (typeof(IRepository<Contact>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)new ContactRepository(context);
            }
            else if (typeof(IRepository<Meeting>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)new MeetingRepository(context);
            }
            else if (typeof(IRepository<Note>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)new NoteRepository(context);
            }
            else if (typeof(IRepository<PersonalInfo>).Name.Equals(typeKey))
            {
                result = (IRepository<TEntity>)new PersonalInfoRepository(context);
            }

            return result;
        }
    }
}