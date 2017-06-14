using Organizer.Common.Entities;
using Organizer.Infrastructure.Database;
using System.Collections.Generic;

namespace Organizer.DAL.Abstract
{
    public interface IContactRepository : IRepository<Contact>
    {
        IEnumerable<Contact> FilterByPhone(int userId, string phone);

        IEnumerable<Contact> FilterBySocialInfo(int userId, SocialInfo socialInfo);

        IEnumerable<Contact> FilterByFirstName(int userId, string firstName);

        IEnumerable<Contact> FilterByLastName(int userId, string lastName);

        IEnumerable<Contact> FilterByMiddleName(int userId, string middleName);

        IEnumerable<Contact> FilterByPersonalInfo(int userId, PersonalInfo info);

        Contact FindByNickName(int userId, string nickname);

        Contact FindByPrimaryPhone(int userId, string phone);

        IEnumerable<Contact> FilterByEmail(int userId, string email);
    }
}