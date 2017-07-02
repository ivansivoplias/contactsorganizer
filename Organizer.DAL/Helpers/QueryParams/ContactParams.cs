using Organizer.Common.Entities;
using System.Data.SqlClient;

namespace Organizer.DAL.Helpers
{
    public static class ContactParams
    {
        public static SqlParameter[] GetFilterBySocialInfoParams(int userId, string appId)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@AppId", appId.MakeLikeExpression())
            };
        }

        public static SqlParameter[] GetFilterByAppInfoLikeParams(int userId, SocialInfo info)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@AppName", info.AppName.MakeLikeExpression()),
                new SqlParameter("@AppId", info.AppId.MakeLikeExpression())
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

        public static SqlParameter[] GetFindContactsByPrimaryPhoneParams(int userId, string phone)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Phone", phone)
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

        public static SqlParameter[] GetFilterByPersonalInfoParams(int userId, string info)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@FirstName", info.MakeLikeExpression()),
                new SqlParameter("@LastName", info.MakeLikeExpression()),
                new SqlParameter("@MiddleName", info.MakeLikeExpression()),
                new SqlParameter("@NickName", info.MakeLikeExpression()),
                new SqlParameter("@Email", info.MakeLikeExpression())
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