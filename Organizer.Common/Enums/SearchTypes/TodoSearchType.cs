using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Common.Enums.SearchTypes
{
    public enum TodoSearchType
    {
        Default = 0,
        ByCreationDate,
        ByLastChangeDate,
        ByState,
        ByPriority,
        CreatedBetween,
        ByStartDate,
        ByEndDate
    }
}