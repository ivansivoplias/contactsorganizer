using Organizer.Common.Pagination;

namespace Organizer.Common.Helpers
{
    public static class PaginationHelper
    {
        public static Page CheckPaginationAndAdoptValues(Page page)
        {
            int pageSize = page.PageSize;

            if (page.PageSize > page.TotalCount && page.TotalCount > 0)
            {
                pageSize = page.TotalCount;
            }

            return new Page(page.TotalCount, page.PageNumber, pageSize);
        }

        public static int GetPagesCount(int totalCount, int numberOnPage)
        {
            var pages = totalCount / numberOnPage;
            var rest = totalCount % numberOnPage;
            if (rest != 0)
            {
                pages++;
            }

            return pages;
        }
    }
}