using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace Organizer.DAL.Repository
{
    public class PersonalInfoRepository : RepositoryBase<PersonalInfo>
    {
        private const string TableName = "PersonalInfo";
        private DataTable _personalInfoTable;

        public PersonalInfoRepository(IDbContext context) : base(context, "PersonalInfos")
        {
            _personalInfoTable = _dataSet.Tables[TableName];
        }

        public override void Create(PersonalInfo entity)
        {
            var info = Get(entity.Nickname);
            if (info == null)
            {
                DataRow infoRow = _personalInfoTable.NewRow();
                infoRow["Name"] = entity.Name;
                infoRow["Lastname"] = entity.Lastname;
                infoRow["MiddleName"] = entity.MiddleName;
                infoRow["Nickname"] = entity.Nickname;
                infoRow["Skype"] = entity.Skype;
                infoRow["Email"] = entity.Email;
                _personalInfoTable.Rows.Add(infoRow);
            }
            else
            {
                throw new Exception($"Personal info with nickname {entity.Nickname} already exists. Cannot add existing personal info to table.");
            }
        }

        public override void Delete(PersonalInfo entity)
        {
            Delete(entity.Id);
        }

        public override void Delete(int id)
        {
            try
            {
                var datarow = _personalInfoTable.Rows.Find(id);
                _personalInfoTable.Rows.Remove(datarow);
            }
            catch { }
        }

        public override PersonalInfo Get(int id)
        {
            PersonalInfo result = null;
            try
            {
                var datarow = _personalInfoTable.Rows.Find(id);
                result = Map(datarow);
            }
            catch { }

            return result;
        }

        public override PersonalInfo Get(object key)
        {
            PersonalInfo result = null;
            var stringKey = key as string;
            if (!string.IsNullOrEmpty(stringKey))
            {
                DataRow[] selected = _personalInfoTable.Select($"Nickname = {stringKey}");
                if (selected != null && selected.Length == 1)
                {
                    result = Map(selected[0]);
                }
            }
            return result;
        }

        public override ICollection<PersonalInfo> GetAll()
        {
            var result = new List<PersonalInfo>();
            var dataRows = _personalInfoTable.Select();
            if (dataRows != null && dataRows.Length > 0)
            {
                foreach (var row in dataRows)
                {
                    result.Add(Map(row));
                }
            }
            return result;
        }

        public override PersonalInfo Map(IDataRecord record)
        {
            var id = int.Parse(record["PersonalInfoId"] as string);
            var name = record["Name"] as string;
            var middleName = record["MiddleName"] as string;
            var lastName = record["Lastname"] as string;
            var nickName = record["Nickname"] as string;
            var skype = record["Skype"] as string;
            var email = record["Email"] as string;

            Func<string, string> comparer = x => x.Equals("null", StringComparison.CurrentCultureIgnoreCase) ? null : x;

            name = comparer(name);
            middleName = comparer(middleName);
            lastName = comparer(lastName);
            email = comparer(email);
            skype = comparer(skype);
            nickName = comparer(nickName);

            var personalInfo = new PersonalInfo()
            {
                Name = name,
                MiddleName = middleName,
                Lastname = lastName,
                Nickname = nickName,
                Email = email,
                Id = id,
                Skype = skype
            };
            return personalInfo;
        }

        public override PersonalInfo Map(DataRow row)
        {
            var id = int.Parse(row["PersonalInfoId"] as string);
            var name = row["Name"] as string;
            var middleName = row["MiddleName"] as string;
            var lastName = row["Lastname"] as string;
            var nickName = row["Nickname"] as string;
            var skype = row["Skype"] as string;
            var email = row["Email"] as string;

            Func<string, string> comparer = x => x.Equals("null", StringComparison.CurrentCultureIgnoreCase) ? null : x;

            name = comparer(name);
            middleName = comparer(middleName);
            lastName = comparer(lastName);
            email = comparer(email);
            skype = comparer(skype);
            nickName = comparer(nickName);

            var personalInfo = new PersonalInfo()
            {
                Name = name,
                MiddleName = middleName,
                Lastname = lastName,
                Nickname = nickName,
                Email = email,
                Id = id,
                Skype = skype
            };
            return personalInfo;
        }

        public override ICollection<PersonalInfo> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(PersonalInfo entity)
        {
            try
            {
                var row = _personalInfoTable.Rows.Find(entity.Id);
                row.BeginEdit();
                row["Name"] = entity.Name;
                row["Lastname"] = entity.Lastname;
                row["MiddleName"] = entity.MiddleName;
                row["Nickname"] = entity.Nickname;
                row["Skype"] = entity.Skype;
                row["Email"] = entity.Email;
                row.EndEdit();
            }
            catch (MissingPrimaryKeyException) { }
        }
    }
}