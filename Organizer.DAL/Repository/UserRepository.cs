using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Organizer.DAL.Repository
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(IDbContext context) : base(context, "Users")
        {
        }

        public override void Create(User entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override User Get(int id)
        {
            throw new NotImplementedException();
        }

        public override User Get(object key)
        {
            throw new NotImplementedException();
        }

        public override ICollection<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public override User Map(IDataRecord record)
        {
            var user = new User();
            user.Id = int.Parse(record["UserId"] as string);
            user.Username = record["Username"] as string;
            user.PasswordHash = record["Password"] as string;
            return user;
        }

        public override ICollection<User> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}