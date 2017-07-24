namespace Organizer.DAL.Helpers
{
    public static class MeetingQueries
    {
        public const string MeetingTable = "dbo.Meetings";
        public const string MeetingId = "MeetingId";

        public static string GetFilterByMeetingDateQuery()
        {
            return "SELECT Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingType, MeetingDate," +
                " NotificationDate, SendNotifications, NotificationTime, MeetingPlace, MeetingTime, Meetings.UserId "
                + $"FROM {MeetingTable} "
                + "WHERE UserId = @UserId AND MeetingDate = @MeetingDate";
        }

        public static string GetFilterByMeetingName()
        {
            return "SELECT Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingType, MeetingDate," +
                " NotificationDate, SendNotifications, NotificationTime, MeetingPlace, MeetingTime, Meetings.UserId "
                + $"FROM {MeetingTable} "
                + "WHERE UserId = @UserId AND MeetingName LIKE @MeetingName";
        }

        public static string GetInsertQuery()
        {
            return $"INSERT INTO {MeetingTable} (Description, MeetingType, MeetingName, MeetingDate, " +
                "NotificationDate, SendNotifications, NotificationTime, MeetingPlace, MeetingTime, UserId)" +
                " VALUES(@Description, @MeetingType, @MeetingName, @MeetingDate, " +
                "@NotificationDate, @SendNotifications, @NotificationTime, @MeetingPlace, @MeetingTime, @UserId)";
        }

        public static string GetUpdateQuery()
        {
            return $"UPDATE {MeetingTable} SET MeetingName = @MeetingName," +
                " Description = @Description, MeetingType = @MeetingType, MeetingDate = @MeetingDate, " +
                "NotificationDate = @NotificationDate, SendNotifications = @SendNotifications, NotificationTime = @NotificationTime," +
                " MeetingPlace = @MeetingPlace, MeetingTime = @MeetingTime, UserId = @Userid" +
                $" WHERE {MeetingId} = @{MeetingId}";
        }

        public static string GetDeleteQuery()
        {
            return $"DELETE FROM {MeetingTable} WHERE {MeetingId} = @{MeetingId}";
        }

        public static string GetGetByIdQuery()
        {
            return "SELECT TOP 1 Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingType, MeetingDate," +
                " NotificationDate, SendNotifications, NotificationTime, MeetingPlace, MeetingTime, Meetings.UserId "
                + $"FROM {MeetingTable}" +
                $" WHERE {MeetingId} = @{MeetingId}";
        }

        public static string GetUserMeetingsQuery()
        {
            return "SELECT Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingType, MeetingDate," +
                " NotificationDate, SendNotifications, NotificationTime, MeetingPlace, MeetingTime, Meetings.UserId "
                + $"FROM {MeetingTable}" +
                " WHERE UserId = @UserId";
        }

        public static string GetFindByMeetingNameQuery()
        {
            return "SELECT TOP 1 Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingType, MeetingDate," +
                " NotificationDate, SendNotifications, NotificationTime, MeetingPlace, MeetingTime, Meetings.UserId "
                + $"FROM {MeetingTable}" +
                " WHERE UserId = @UserId AND MeetingName = @MeetingName";
        }

        public static string GetAllQuery()
        {
            return "SELECT Meetings.MeetingId, MeetingName," +
                " Meetings.Description, MeetingType, MeetingDate," +
                " NotificationDate, SendNotifications, NotificationTime, MeetingPlace, MeetingTime, Meetings.UserId "
                + $"FROM {MeetingTable}";
        }
    }
}