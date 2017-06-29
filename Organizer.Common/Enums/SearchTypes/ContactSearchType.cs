using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Common.Enums.SearchTypes
{
    public enum ContactSearchType
    {
        Default = 0,
        ByPrimaryPhone,
        ByNickName,
        ByEmail,
        ByFirstName,
        ByLastName,
        ByMiddleName,
        BySocialInfo
    }
}