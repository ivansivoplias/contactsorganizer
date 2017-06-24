namespace Organizer.DAL.Helpers
{
    public static class SocialInfoQueries
    {
        public const string SocialInfoTable = "dbo.SocialInfo";
        public const string SocialInfoId = "SocialInfoId";

        public static string GetInsertQuery()
        {
            return $"INSERT INTO {SocialInfoTable} (ContactId, AppName, AppId)" +
                " VALUES(@ContactId, @AppName, @AppId)";
        }

        public static string GetUpdateQuery()
        {
            return $"UPDATE {SocialInfoTable} SET AppName = @AppName, AppId = @AppId" +
                $" WHERE {SocialInfoId} = @{SocialInfoId}";
        }

        public static string GetDeleteQuery()
        {
            return $"DELETE FROM {SocialInfoTable} WHERE {SocialInfoId} = @{SocialInfoId}";
        }

        public static string GetGetByIdQuery()
        {
            return $"SELECT TOP 1 * FROM {SocialInfoTable} WHERE {SocialInfoId} = @{SocialInfoId}";
        }

        public static string GetAllQuery()
        {
            return $"SELECT * FROM {SocialInfoTable}";
        }

        public static string GetContactSocialsQuery()
        {
            return $"SELECT * FROM {SocialInfoTable} WHERE ContactId = @ContactId";
        }
    }
}