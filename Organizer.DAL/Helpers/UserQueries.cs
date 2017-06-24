namespace Organizer.DAL.Helpers
{
    public static class UserQueries
    {
        public const string UserTable = "dbo.Users";
        public const string UserId = "UserId";

        public static string GetInsertQuery()
        {
            return $"INSERT INTO {UserTable} (Login, Password)" +
                " VALUES(@Login, @Password)";
        }

        public static string GetUpdateQuery()
        {
            return $"UPDATE {UserTable} SET Login = @Login," +
                $" Password = @Password WHERE {UserId} = @{UserId}";
        }

        public static string GetDeleteQuery()
        {
            return $"DELETE FROM {UserTable} WHERE {UserId} = @{UserId}";
        }

        public static string GetGetByIdQuery()
        {
            return "SELECT TOP 1 UserId, Login, Password "
                + $"FROM {UserTable} " +
                $"WHERE {UserId} = @{UserId}";
        }

        public static string GetAllQuery()
        {
            return "SELECT UserId, Login, Password "
                + $"FROM {UserTable}";
        }
    }
}