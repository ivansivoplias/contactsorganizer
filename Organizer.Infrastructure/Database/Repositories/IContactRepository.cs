using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface IContactRepository : IRepository<Contact>
    {
        IEnumerable<Contact> FilterBySocialInfoAppIdLike(int userId, string appId, int? pageSize = null, int? page = null);

        IEnumerable<Contact> FilterByFirstNameStartsWith(int userId, string firstName, int? pageSize = null, int? page = null);

        IEnumerable<Contact> FilterByLastNameStartsWith(int userId, string lastName, int? pageSize = null, int? page = null);

        IEnumerable<Contact> FilterByMiddleNameStartsWith(int userId, string middleName, int? pageSize = null, int? page = null);

        IEnumerable<Contact> FilterByPersonalInfo(int userId, string personalInfo, int? pageSize = null, int? page = null);

        IEnumerable<Contact> FilterByAppInfoLike(int userId, SocialInfo info, int? pageSize = null, int? page = null);

        Contact FindByNickName(int userId, string nickname);

        IEnumerable<Contact> FindContactsByPrimaryPhone(int userId, string phone, int? pageSize = null, int? page = null);

        Contact FindByPrimaryPhone(int userId, string phone);

        IEnumerable<Contact> GetUserContacts(int userId, int? pageSize = null, int? page = null);

        IEnumerable<Contact> FilterByEmailStartsWith(int userId, string email, int? pageSize = null, int? page = null);
    }
}