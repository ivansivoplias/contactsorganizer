using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface ISocialInfoRepository : IRepository<SocialInfo>
    {
        IEnumerable<SocialInfo> GetContactSocials(int contactId, int pageSize = 10, int page = 1);
    }
}