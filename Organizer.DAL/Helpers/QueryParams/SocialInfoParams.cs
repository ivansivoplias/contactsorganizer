using System.Data.SqlClient;

namespace Organizer.DAL.Helpers
{
    public static class SocialInfoParams
    {
        public static SqlParameter[] GetGetContactSocialsParams(int contactId)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@ContactId", contactId)
            };
        }

        public static SqlParameter[] GetSocialsByAppNameParams(int contactId, string appName)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@ContactId", contactId),
                new SqlParameter("@AppName", appName)
            };
        }

        public static SqlParameter[] GetFindSocialParams(int contactId, string appName, string appId)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@ContactId", contactId),
                new SqlParameter("@AppName", appName),
                new SqlParameter("@AppId", appId)
            };
        }
    }
}