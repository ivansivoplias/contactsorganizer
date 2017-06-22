using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface IContactRepository : IRepository<Contact>
    {
        IEnumerable<Contact> FilterBySocialInfoAppIdLike(int userId, SocialInfo socialInfo);

        IEnumerable<Contact> FilterByFirstNameStartsWith(int userId, string firstName);

        IEnumerable<Contact> FilterByLastNameStartsWith(int userId, string lastName);

        IEnumerable<Contact> FilterByMiddleNameStartsWith(int userId, string middleName);

        IEnumerable<Contact> FilterByPersonalInfo(int userId, PersonalInfo info);

        Contact FindByNickName(int userId, string nickname);

        Contact FindByPrimaryPhone(int userId, string phone);

        IEnumerable<Contact> FilterByEmailStartsWith(int userId, string email);
    }
}