namespace Organizer.DAL.Helpers
{
    public static class MeetingQueries
    {
        public const string MeetingTable = "dbo.Meetings";
        public const string MeetingId = "MeetingId";

        public static string GetFilterByMeetingDateQuery()
        {
            return "SELECT Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingDate," +
                " NotificationDate, SendNotifications, Meetings.UserId "
                + $"FROM {MeetingTable}"
                + "WHERE UserId = @UserId AND MeetingDate = @MeetingDate";
        }

        public static string GetFilterByMeetingName()
        {
            return "SELECT Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingDate," +
                " NotificationDate, SendNotifications, Meetings.UserId "
                + $"FROM {MeetingTable}"
                + "WHERE UserId = @UserId AND MeetingName LIKE @MeetingName";
        }

        public static string GetInsertQuery()
        {
            return $"INSERT INTO {MeetingTable} (Description, MeetingName, MeetingDate, " +
                "NotificationDate, SendNotifications, UserId)" +
                " VALUES(@Description, @MeetingName, @MeetingDate, " +
                "@NotificationDate, @SendNotifications, @UserId)";
        }

        public static string GetUpdateQuery()
        {
            return $"UPDATE {MeetingTable} SET MeetingName = @MeetingName," +
                " Description = @Description, MeetingDate = @MeetingDate, " +
                "NotificationDate = @NotificationDate, SendNotifications = @SendNotifications," +
                " UserId = @Userid" +
                $" WHERE {MeetingId} = @{MeetingId}";
        }

        public static string GetDeleteQuery()
        {
            return $"DELETE FROM {MeetingTable} WHERE {MeetingId} = @{MeetingId}";
        }

        public static string GetGetByIdQuery()
        {
            return "SELECT TOP 1 Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingDate," +
                " NotificationDate, SendNotifications, Meetings.UserId "
                + $"FROM {MeetingTable}" +
                $" WHERE {MeetingId} = @{MeetingId}";
        }

        public static string GetUserMeetingsQuery()
        {
            return "SELECT Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingDate," +
                " NotificationDate, SendNotifications, Meetings.UserId "
                + $"FROM {MeetingTable}" +
                " WHERE UserId = @UserId";
        }

        public static string GetFindByMeetingNameQuery()
        {
            return "SELECT TOP 1 Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingDate," +
                " NotificationDate, SendNotifications, Meetings.UserId "
                + $"FROM {MeetingTable}" +
                " WHERE UserId = @UserId AND MeetingName = @MeetingName";
        }

        public static string GetAllQuery()
        {
            return "SELECT Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingDate," +
                " NotificationDate, SendNotifications, Meetings.UserId "
                + $"FROM {MeetingTable}";
        }
    }
}