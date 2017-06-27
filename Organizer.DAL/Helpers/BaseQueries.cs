namespace Organizer.DAL.Helpers
{
    public static class BaseQueries
    {
        public static string GetFilteredCountQuery(string filterSql)
        {
            return $"SELECT COUNT(*) FROM ({filterSql}) AS FilterQueryResult";
        }

        public static string GetCountQuery(string tablename)
        {
            return $"SELECT COUNT(*) FROM {tablename}";
        }
    }
}