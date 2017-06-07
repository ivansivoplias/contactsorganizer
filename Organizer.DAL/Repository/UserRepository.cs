using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace Organizer.DAL.Repository
{
    public class UserRepository : RepositoryBase<User>
    {
        private const string TableName = "Users";
        private DataTable _userTable;

        public UserRepository(IDbContext context) : base(context, TableName)
        {
            _userTable = _dataSet.Tables[TableName];
        }

        public override void Create(User entity)
        {
            var user = Get(entity.Username);
            if (user == null)
            {
                DataRow userRow = _userTable.NewRow();
                userRow["Username"] = entity.Username;
                userRow["Password"] = entity.PasswordHash;
                _userTable.Rows.Add(userRow);
            }
            else
            {
                throw new Exception($"User with name {entity.Username} already exists. Cannot add existing user to table.");
            }
        }

        public override void Delete(User entity)
        {
            Delete(entity.Id);
        }

        public override void Delete(int id)
        {
            try
            {
                var datarow = _userTable.Rows.Find(id);
                _userTable.Rows.Remove(datarow);
            }
            catch { }
        }

        public override User Get(int id)
        {
            User result = null;
            try
            {
                var datarow = _userTable.Rows.Find(id);
                result = Map(datarow);
            }
            catch { }

            return result;
        }

        public override User Get(object key)
        {
            User result = null;
            var stringKey = key as string;
            if (!string.IsNullOrEmpty(stringKey))
            {
                DataRow[] selected = _userTable.Select($"Username = {stringKey}");
                if (selected != null && selected.Length == 1)
                {
                    result = Map(selected[0]);
                }
            }
            return result;
        }

        public override ICollection<User> GetAll()
        {
            var result = new List<User>();
            var dataRows = _userTable.Select();
            if (dataRows != null && dataRows.Length > 0)
            {
                foreach (var row in dataRows)
                {
                    result.Add(Map(row));
                }
            }
            return result;
        }

        public override User Map(IDataRecord record)
        {
            var user = new User();
            user.Id = int.Parse(record["UserId"] as string);
            user.Username = record["Username"] as string;
            user.PasswordHash = record["Password"] as string;
            return user;
        }

        public override User Map(DataRow row)
        {
            var user = new User();
            user.Id = int.Parse(row["UserId"] as string);
            user.Username = row["Username"] as string;
            user.PasswordHash = row["Password"] as string;
            return user;
        }

        public override ICollection<User> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(User entity)
        {
            try
            {
                var row = _userTable.Rows.Find(entity.Id);
                row.BeginEdit();
                row["Username"] = entity.Username;
                row["Password"] = entity.PasswordHash;
                row.EndEdit();
            }
            catch { }
        }
    }
}