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
    public class PersonalInfoRepository : RepositoryBase<PersonalInfo>
    {
        public PersonalInfoRepository(IDbContext context) : base(context, "PersonalInfos")
        {
        }

        public override void Create(PersonalInfo entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(PersonalInfo entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override PersonalInfo Get(int id)
        {
            throw new NotImplementedException();
        }

        public override PersonalInfo Get(object key)
        {
            throw new NotImplementedException();
        }

        public override ICollection<PersonalInfo> GetAll()
        {
            throw new NotImplementedException();
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

        public override ICollection<PersonalInfo> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(PersonalInfo entity)
        {
            throw new NotImplementedException();
        }
    }
}