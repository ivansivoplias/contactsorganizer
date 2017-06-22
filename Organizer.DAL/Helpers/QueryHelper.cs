using System.Data.SqlClient;

namespace Organizer.DAL.Helpers
{
    public static class QueryHelper
    {
        public static void SetupCommand(SqlCommand command, string query, params SqlParameter[] parameters)
        {
            command.CommandText = query;
            foreach (var param in parameters)
            {
                command.Parameters.Add(param);
            }
        }

        public static string MakeLikeExpression(this string source)
        {
            return $"%{source}%";
        }
    }
}