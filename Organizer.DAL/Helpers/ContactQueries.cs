namespace Organizer.DAL.Helpers
{
    public static class ContactQueries
    {
        public const string ContactTable = "dbo.Contacts";
        public const string SocialInfoTable = "dbo.SocialInfo";
        public const string PersonalInfoTable = "dbo.PersonalInfo";
        public const string PersonalInfoId = "PersonalInfoId";
        public const string ContactId = "ContactId";

        public static string GetFilterBySocialInfoQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {ContactTable} " +
                $"INNER JOIN {SocialInfoTable} ON {ContactTable}.{ContactId} = {SocialInfoTable}.ContactId "
                + "WHERE UserId = @UserId AND AppName = @AppName AND AppId LIKE @AppId";
        }

        public static string GetFilterByFirstNameQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {ContactTable} " +
                $"INNER JOIN {PersonalInfoTable} ON {ContactTable}.{ContactId} = {PersonalInfoTable}.{PersonalInfoId}"
                + " WHERE UserId = @UserId AND FirstName LIKE @FirstName";
        }

        public static string GetFilterByLastNameQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {ContactTable} " +
                $"INNER JOIN {PersonalInfoTable} ON {ContactTable}.{ContactId} = {PersonalInfoTable}.{PersonalInfoId}"
                + " WHERE UserId = @UserId AND Lastname LIKE @LastName";
        }

        public static string GetFilterByMiddleNameQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {ContactTable} " +
                $"INNER JOIN {PersonalInfoTable} ON {ContactTable}.{ContactId} = {PersonalInfoTable}.{PersonalInfoId}"
                + " WHERE UserId = @UserId AND MiddleName LIKE @MiddleName";
        }

        public static string GetFilterByPersonalInfoQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {ContactTable} " +
                $"INNER JOIN {PersonalInfoTable} ON {ContactTable}.{ContactId} = {PersonalInfoTable}.{PersonalInfoId}"
                + " WHERE UserId = @UserId AND FirstName = @FirstName" +
                " AND Lastname = @LastName AND" +
                " MiddleName = @MiddleName AND Nickname = @NickName AND Email = @Email";
        }

        public static string GetFindByNicknameQuery()
        {
            return $"SELECT TOP 1 Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {ContactTable} " +
                $"INNER JOIN {PersonalInfoTable} ON {ContactTable}.{ContactId} = {PersonalInfoTable}.{PersonalInfoId}"
                + " WHERE UserId = @UserId AND Nickname = @NickName";
        }

        public static string GetFindByPrimaryPhoneQuery()
        {
            return $"SELECT TOP 1 Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {ContactTable}" +
                " WHERE UserId = @UserId AND PrimaryPhone = @Phone";
        }

        public static string GetFilterByEmailQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {ContactTable} " +
                $"INNER JOIN {PersonalInfoTable} ON {ContactTable}.{ContactId} = {PersonalInfoTable}.{PersonalInfoId}"
                + " WHERE UserId = @UserId AND Email LIKE @Email";
        }

        public static string GetGetByIdQuery()
        {
            return $"SELECT TOP 1 Contacts.ContactId AS ContactId, PrimaryPhone, UserId" +
                $" FROM {ContactTable} WHERE {ContactId} = @{ContactId}";
        }

        public static string GetGetAllQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {ContactTable}";
        }

        public static string GetUpdateQuery()
        {
            return $"UPDATE {ContactTable} SET PrimaryPhone = @PrimaryPhone," +
                $" WHERE {ContactId} = @{ContactId}";
        }

        public static string GetInsertQuery()
        {
            return $"INSERT INTO {ContactTable} (PrimaryPhone, UserId)" +
                " VALUES(@PrimaryPhone, @UserId)";
        }

        public static string GetDeleteQuery()
        {
            return $"DELETE FROM {ContactTable} WHERE {ContactId} = @{ContactId}";
        }
    }
}