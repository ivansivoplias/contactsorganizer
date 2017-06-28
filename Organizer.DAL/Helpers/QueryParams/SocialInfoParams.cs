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
    }
}