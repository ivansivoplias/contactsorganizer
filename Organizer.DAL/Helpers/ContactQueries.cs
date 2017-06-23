namespace Organizer.DAL.Helpers
{
    public static class ContactQueries
    {
        private const string _contactTable = "dbo.Contacts";
        private const string _socialInfoTable = "dbo.SocialInfo";
        private const string _personalInfoTable = "dbo.PersonalInfo";
        private const string _personalInfoId = "PersonalInfoId";
        private const string _contactId = "ContactId";

        public static string GetFilterBySocialInfoQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {_contactTable} " +
                $"INNER JOIN {_socialInfoTable} ON {_contactTable}.{_contactId} = {_socialInfoTable}.ContactId "
                + "WHERE UserId = @UserId AND AppName = @AppName AND AppId LIKE @AppId";
        }

        public static string GetFilterByFirstNameQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {_contactTable} " +
                $"INNER JOIN {_personalInfoTable} ON {_contactTable}.{_contactId} = {_personalInfoTable}.{_personalInfoId}"
                + " WHERE UserId = @UserId AND FirstName LIKE @FirstName";
        }

        public static string GetFilterByLastNameQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {_contactTable} " +
                $"INNER JOIN {_personalInfoTable} ON {_contactTable}.{_contactId} = {_personalInfoTable}.{_personalInfoId}"
                + " WHERE UserId = @UserId AND Lastname LIKE @LastName";
        }

        public static string GetFilterByMiddleNameQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {_contactTable} " +
                $"INNER JOIN {_personalInfoTable} ON {_contactTable}.{_contactId} = {_personalInfoTable}.{_personalInfoId}"
                + " WHERE UserId = @UserId AND MiddleName LIKE @MiddleName";
        }

        public static string GetFilterByPersonalInfoQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {_contactTable} " +
                $"INNER JOIN {_personalInfoTable} ON {_contactTable}.{_contactId} = {_personalInfoTable}.{_personalInfoId}"
                + " WHERE UserId = @UserId AND FirstName = @FirstName" +
                " AND Lastname = @LastName AND" +
                " MiddleName = @MiddleName AND Nickname = @NickName AND Email = @Email";
        }

        public static string GetFindByNicknameQuery()
        {
            return $"SELECT TOP 1 Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {_contactTable} " +
                $"INNER JOIN {_personalInfoTable} ON {_contactTable}.{_contactId} = {_personalInfoTable}.{_personalInfoId}"
                + " WHERE UserId = @UserId AND Nickname = @NickName";
        }

        public static string GetFindByPrimaryPhoneQuery()
        {
            return $"SELECT TOP 1 Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {_contactTable}" +
                " WHERE UserId = @UserId AND PrimaryPhone = @Phone";
        }

        public static string GetFilterByEmailQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {_contactTable} " +
                $"INNER JOIN {_personalInfoTable} ON {_contactTable}.{_contactId} = {_personalInfoTable}.{_personalInfoId}"
                + " WHERE UserId = @UserId AND Email LIKE @Email";
        }

        public static string GetGetByIdQuery()
        {
            return $"SELECT TOP 1 Contacts.ContactId AS ContactId, PrimaryPhone, UserId" +
                $" FROM {_contactTable} WHERE {_contactId} = @{_contactId}";
        }

        public static string GetGetAllQuery()
        {
            return $"SELECT Contacts.ContactId AS ContactId, PrimaryPhone, UserId FROM {_contactTable}";
        }

        public static string GetUpdateQuery()
        {
            return $"UPDATE {_contactTable} SET PrimaryPhone = @PrimaryPhone," +
                $" WHERE {_contactId} = @{_contactId}";
        }

        public static string GetInsertQuery()
        {
            return $"INSERT INTO {_contactTable} (PrimaryPhone, UserId)" +
                " VALUES(@PrimaryPhone, @UserId)";
        }

        public static string GetDeleteQuery()
        {
            return $"DELETE FROM {_contactTable} WHERE {_contactId} = @{_contactId}";
        }
    }
}