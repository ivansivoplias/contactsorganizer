using Organizer.Common.Pagination;
using System;

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

            if (pageSize > totalCount && totalCount > 0)
            {
                pageSize = totalCount;
            }

            //if (pageSize * (pageNumber - 1) > totalCount && totalCount > 0)
            //{
            //    pageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
            //}

            resultPage = new Page(totalCount, pageNumber, pageSize);

            return resultPage;
        }
    }
}