namespace Organizer.DAL.Helpers
{
    public static class SocialInfoQueries
    {
        public const string SocialInfoTable = "dbo.SocialInfo";
        public const string SocialInfoId = "SocialInfoId";
        public const string ContactTable = "dbo.Contacts";
        public const string ContactId = "ContactId";

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
            return "SELECT TOP 1 SocialInfo.SocialInfoId, SocialInfo.ContactId, AppName, AppId "
                + $"FROM {SocialInfoTable} "
                + $"WHERE {SocialInfoId} = @{SocialInfoId}";
        }

        public static string GetFindSocialQuery()
        {
            return "SELECT TOP 1 SocialInfo.SocialInfoId, SocialInfo.ContactId, AppName, AppId "
                + $"FROM {SocialInfoTable} "
                + "WHERE ContactId = @ContactId AND AppName = @AppName AND AppId = @AppId";
        }

        public static string GetAllQuery()
        {
            return "SELECT SocialInfo.SocialInfoId, SocialInfo.ContactId, AppName, AppId "
                + $"FROM {SocialInfoTable}";
        }

        public static string GetContactSocialsQuery()
        {
            return "SELECT SocialInfo.SocialInfoId, SocialInfo.ContactId, AppName, AppId "
                + $"FROM {SocialInfoTable} " +
                "WHERE ContactId = @ContactId";
        }

        public static string GetSocialsByAppNameQuery()
        {
            return "SELECT SocialInfo.SocialInfoId, SocialInfo.ContactId, AppName, AppId "
                + $"FROM {SocialInfoTable} INNER JOIN {ContactTable} ON {ContactTable}.{ContactId} = {SocialInfoTable}.ContactId " +
                $"WHERE {SocialInfoTable}.ContactId = @ContactId AND AppName = @AppName";
        }
    }
}