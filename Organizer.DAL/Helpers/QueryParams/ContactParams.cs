using Organizer.Common.Entities;
using System.Data.SqlClient;

namespace Organizer.DAL.Helpers
{
    public static class ContactParams
    {
        public static SqlParameter[] GetFilterBySocialInfoParams(int userId, SocialInfo socialInfo)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@AppId", socialInfo.AppId.MakeLikeExpression()),
                new SqlParameter("@AppName", socialInfo.AppName)
            };
        }

        public static SqlParameter[] GetFilterByFirstNameParams(int userId, string firstName)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@FirstName", firstName.MakeStartsWithLikeExpression())
            };
        }

        public static SqlParameter[] GetFilterByLastnameParams(int userId, string lastName)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@LastName", lastName.MakeStartsWithLikeExpression())
            };
        }

        public static SqlParameter[] GetFilterByMiddleNameParams(int userId, string middleName)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@MiddleName", middleName.MakeStartsWithLikeExpression())
            };
        }

        public static SqlParameter[] GetFilterByPersonalInfoParams(int userId, PersonalInfo info)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@FirstName", info.FirstName),
                new SqlParameter("@LastName", info.Lastname),
                new SqlParameter("@MiddleName", info.MiddleName),
                new SqlParameter("@NickName", info.Nickname),
                new SqlParameter("@Email", info.Email)
            };
        }

        public static SqlParameter[] GetFilterByEmailParams(int userId, string email)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Email", email.MakeStartsWithLikeExpression())
            };
        }

        public static SqlParameter[] GetGetUserContactsParams(int userId)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };
        }
    }
}