using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface IContactRepository : IRepository<Contact>
    {
        IEnumerable<Contact> FilterBySocialInfoAppIdLike(int userId, SocialInfo socialInfo, int pageSize = 10, int page = 1);

        IEnumerable<Contact> FilterByFirstNameStartsWith(int userId, string firstName, int pageSize = 10, int page = 1);

        IEnumerable<Contact> FilterByLastNameStartsWith(int userId, string lastName, int pageSize = 10, int page = 1);

        IEnumerable<Contact> FilterByMiddleNameStartsWith(int userId, string middleName, int pageSize = 10, int page = 1);

        IEnumerable<Contact> FilterByPersonalInfo(int userId, PersonalInfo info, int pageSize = 10, int page = 1);

        Contact FindByNickName(int userId, string nickname);

        Contact FindByPrimaryPhone(int userId, string phone);

        IEnumerable<Contact> GetUserContacts(int userId, int pageSize = 10, int page = 1);

        IEnumerable<Contact> FilterByEmailStartsWith(int userId, string email, int pageSize = 10, int page = 1);
    }
}