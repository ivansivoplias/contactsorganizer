namespace Organizer.DAL.Helpers
{
    public static class PersonalInfoQueries
    {
        public const string PersonalInfoTable = "dbo.PersonalInfo";
        public const string PersonalInfoId = "PersonalInfoId";

        public static string GetInsertQuery()
        {
            return $"INSERT INTO {PersonalInfoTable} ({PersonalInfoId}, FirstName, Lastname, MiddleName, " +
                "Nickname, Email)" +
                $" VALUES(@{PersonalInfoId}, @FirstName, @Lastname, @MiddleName, " +
                "@Nickname, @Email)";
        }

        public static string GetUpdateQuery()
        {
            return $"UPDATE {PersonalInfoTable} SET FirstName = @FirstName," +
                " Lastname = @Lastname, MiddleName = @MiddleName, " +
                "Nickname = @Nickname, Email = @Email" +
                $" WHERE {PersonalInfoId} = @{PersonalInfoId}";
        }

        public static string GetDeleteQuery()
        {
            return $"DELETE FROM {PersonalInfoTable} WHERE {PersonalInfoId} = @{PersonalInfoId}";
        }

        public static string GetGetByIdQuery()
        {
            return "SELECT PersonalInfo.PersonalInfoId, PersonalInfo.FirstName," +
                " PersonalInfo.Lastname, PersonalInfo.MiddleName," +
                " PersonalInfo.Nickname, PersonalInfo.Email "
                + $"FROM {PersonalInfoTable}" +
                $" WHERE {PersonalInfoId} = @{PersonalInfoId}";
        }

        public static string GetAllQuery()
        {
            return "SELECT PersonalInfo.PersonalInfoId, PersonalInfo.FirstName," +
                " PersonalInfo.Lastname, PersonalInfo.MiddleName," +
                " PersonalInfo.Nickname, PersonalInfo.Email "
                + $"FROM {PersonalInfoTable}";
        }
    }
}