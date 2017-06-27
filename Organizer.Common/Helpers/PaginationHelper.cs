using Organizer.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Common.Helpers
{
    public static class PaginationHelper
    {
        public static Page CheckPaginationAndAdoptValues(Page page)
        {
            Page resultPage;

            int pageSize = page.PageSize;

            int totalCount = page.TotalCount;

            int pageNumber = page.PageNumber;

            if (pageSize > totalCount)
            {
                pageSize = totalCount;
            }

            if (pageSize * (pageNumber - 1) > totalCount)
            {
                pageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
            }

            resultPage = new Page(totalCount, pageNumber, pageSize);

            return resultPage;
        }
    }
}