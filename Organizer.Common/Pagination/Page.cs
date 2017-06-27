namespace Organizer.Common.Pagination
{
    public struct Page
    {
        public int TotalCount { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public Page(int totalCount, int pageNumber, int pageSize)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}