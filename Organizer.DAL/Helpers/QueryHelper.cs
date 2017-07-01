using System.Data;
using System.Data.SqlClient;

namespace Organizer.DAL.Helpers
{
    public static class QueryHelper
    {
        public static void SetupCommand(SqlCommand command, string query, params SqlParameter[] parameters)
        {
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            foreach (var param in parameters)
            {
                command.Parameters.Add(param);
            }
        }

        public static string MakeLikeExpression(this string source)
        {
            return $"%{source}%";
        }

        public static string MakeStartsWithLikeExpression(this string source)
        {
            return $"{source}%";
        }

        public static string MakeEndsWithLikeExpression(this string source)
        {
            return $"%{source}";
        }

        public static string AddPaging(this string query, string orderBy, int pageSize, int pageNumber)
        {
            return $"{query} ORDER BY {orderBy} " +
                $"OFFSET {pageSize * (pageNumber - 1)} ROWS " +
                $"FETCH NEXT {pageSize} ROWS ONLY";
        }
    }
}