using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Common.Enums.SearchTypes
{
    public enum DiarySearchType
    {
        Default = 0,
        ByCreationDate,
        ByLastChangeDate,
        CreatedBetween,
        ByCaptionLike
    }
}