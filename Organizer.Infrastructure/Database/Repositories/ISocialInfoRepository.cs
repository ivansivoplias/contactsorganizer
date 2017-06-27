using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Database
{
    public interface ISocialInfoRepository : IRepository<SocialInfo>
    {
        IEnumerable<SocialInfo> GetContactSocials(int contactId, int? pageSize = null, int? page = null);
    }
}