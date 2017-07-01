using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface ISocialInfoRepository : IRepository<SocialInfo>
    {
        SocialInfo FindSocial(int contactId, string appName, string appId);

        IEnumerable<SocialInfo> GetContactSocials(int contactId, int? pageSize = null, int? page = null);

        IEnumerable<SocialInfo> GetSocialsByAppName(int contactId, string appName, int? pageSize = null, int? page = null);
    }
}