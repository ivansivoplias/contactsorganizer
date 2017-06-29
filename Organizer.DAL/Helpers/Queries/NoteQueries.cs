namespace Organizer.DAL.Helpers
{
    public static class NoteQueries
    {
        public const string NoteTable = "dbo.Notes";
        public const string NoteId = "NoteId";

        public static string GetFilterByCreationDateQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND CreationDate = @CreationDate AND NoteType = @NoteType";
        }

        public static string GetFilterByLastChangeDateQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND LastChangeDate = @LastChangeDate AND NoteType = @NoteType";
        }

        public static string GetFilterByNoteTypeQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND NoteType = @NoteType";
        }

        public static string GetFilterByStateQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND State = @State AND NoteType = @NoteType";
        }

        public static string GetFilterByPriorityQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND Priority = @Priority AND NoteType = @NoteType";
        }

        public static string GetFilterByCreationBetweenQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND СreationDate BETWEEN @StartLimit AND @EndLimit AND NoteType = @NoteType";
        }

        public static string GetFilterByStartDateQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND StartDate = @StartDate AND NoteType = @NoteType";
        }

        public static string GetFilterByEndDateQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND EndDate = @EndDate AND NoteType = @NoteType";
        }

        public static string GetInsertQuery()
        {
            return $"INSERT INTO {NoteTable} (Caption, NoteText, CreationDate, LastChangeDate, " +
                "NoteType, State, Priority, StartDate, EndDate, UserId)" +
                " VALUES(@Caption, @NoteText, @CreationDate, @LastChangeDate, " +
                "@NoteType, @State, @Priority, @StartDate, @EndDate, @UserId)";
        }

        public static string GetUpdateQuery()
        {
            return $"UPDATE {NoteTable} SET Caption = @Caption, NoteText = @NoteText," +
                " CreationDate = @CreationDate, LastChangeDate = @LastChangeDate, " +
                "NoteType = @NoteType, State = @State, Priority = @Priority," +
                " StartDate = @StartDate, EndDate = @EndDate" +
                $" WHERE {NoteId} = @{NoteId}";
        }

        public static string GetDeleteQuery()
        {
            return $"DELETE FROM {NoteTable} WHERE {NoteId} = @{NoteId}";
        }

        public static string GetGetByIdQuery()
        {
            return "SELECT TOP 1 Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} " +
                $"WHERE {NoteId} = @{NoteId}";
        }

        public static string GetUserNotesQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId";
        }

        public static string GetAllQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} ";
        }

        public static string GetFilterByCaptionQuery()
        {
            return "SELECT Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND Caption LIKE @Caption AND NoteType = @NoteType";
        }

        public static string GetNoteByCaptionQuery()
        {
            return "SELECT TOP 1 Notes.NoteId, Notes.Caption, Notes.NoteText," +
                " Notes.CreationDate, Notes.LastChangeDate, Notes.NoteType," +
                " Notes.State, Notes.Priority, Notes.StartDate, Notes.EndDate, Notes.UserId" +
                $" FROM {NoteTable} "
                + "WHERE UserId = @UserId AND Caption = @Caption";
        }
    }
}