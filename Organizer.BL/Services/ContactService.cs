using Organizer.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organizer.Common.DTO;
using Autofac;
using Organizer.Infrastructure.Database;
using Organizer.DAL.Repository;
using Organizer.Common.Entities;
using AutoMapper;
using System.Data.SqlClient;

namespace Organizer.BL.Services
{
    public class ContactService : IContactService
    {
        private readonly IContainer _container;

        public ContactService(IContainer container)
        {
            _container = container;
        }

        public void AddContact(ContactDto contact)
        {
            var mappedContact = Mapper.Map<Contact>(contact);
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactsRepo = new ContactRepository(unitOfWork);
                using (var transaction = unitOfWork.BeginTransaction())
                {
                    contactsRepo.Insert(mappedContact, transaction);
                    unitOfWork.Commit();
                }

                var added = contactsRepo.FindByPrimaryPhone(contact.UserId, contact.PrimaryPhone);

                using (var transaction = unitOfWork.BeginTransaction())
                {
                    AddPersonalInfo(added.Id, contact.PersonalInfo, unitOfWork);
                    AddSocials(added.Id, contact.Socials, unitOfWork);
                    unitOfWork.Commit();
                }
            }
        }

        public void EditContact(ContactDto contact)
        {
            var mappedContact = Mapper.Map<Contact>(contact);
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactsRepo = new ContactRepository(unitOfWork);
                using (var transaction = unitOfWork.BeginTransaction())
                {
                    contactsRepo.Update(mappedContact, transaction);
                    unitOfWork.Commit();
                }

                using (var transaction = unitOfWork.BeginTransaction())
                {
                    EditPersonalInfo(contact.PersonalInfo, unitOfWork);
                    EditSocials(contact.Id, contact.Socials, unitOfWork);
                    unitOfWork.Commit();
                }
            }
        }

        private void AddPersonalInfo(int contactId, PersonalInfoDto personalInfo, IUnitOfWork unitOfWork)
        {
            var personalRepo = new PersonalInfoRepository(unitOfWork);
            var mapped = Mapper.Map<PersonalInfo>(personalInfo);
            mapped.Id = contactId;
            personalRepo.Insert(mapped, unitOfWork.Transaction);
        }

        private void AddSocials(int contactId, IEnumerable<SocialInfoDto> socials, IUnitOfWork unitOfWork)
        {
            var socialRepo = new SocialInfoRepository(unitOfWork);
            var mappedSocials = Mapper.Map<IEnumerable<SocialInfo>>(socials);
            foreach (var mappedSocial in mappedSocials)
            {
                mappedSocial.ContactId = contactId;
                socialRepo.Insert(mappedSocial, unitOfWork.Transaction);
            }
        }

        private void EditPersonalInfo(PersonalInfoDto personalInfo, IUnitOfWork unitOfWork)
        {
            var mapped = Mapper.Map<PersonalInfo>(personalInfo);
            var personalRepo = new PersonalInfoRepository(unitOfWork);
            personalRepo.Update(mapped, unitOfWork.Transaction);
        }

        private void EditSocials(int contactId, IEnumerable<SocialInfoDto> socials, IUnitOfWork unitOfWork)
        {
            var socialRepo = new SocialInfoRepository(unitOfWork);

            foreach (var socialInfo in socials)
            {
                var dbSocial = socialRepo.GetById(socialInfo.Id);
                var mapped = Mapper.Map<SocialInfo>(socialInfo);
                if (dbSocial != null)
                {
                    socialRepo.Update(mapped, unitOfWork.Transaction);
                }
                else
                {
                    mapped.ContactId = contactId;
                    socialRepo.Insert(mapped, unitOfWork.Transaction);
                }
            }
        }

        public ContactDto FindByNickName(UserDto user, string nickName)
        {
            ContactDto result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContact = contactRepo.FindByNickName(user.Id, nickName);
                result = Mapper.Map<ContactDto>(dbContact);
                GetPersonalAndSocialsForContact(result, unitOfWork);
            }

            return result;
        }

        private void GetPersonalAndSocialsForContact(ContactDto contact, IUnitOfWork unitOfWork)
        {
            var personalRepo = new PersonalInfoRepository(unitOfWork);
            var socialRepo = new SocialInfoRepository(unitOfWork);

            var personal = personalRepo.GetById(contact.Id);
            contact.PersonalInfo = Mapper.Map<PersonalInfoDto>(personal);

            var socials = socialRepo.GetContactSocials(contact.Id);

            contact.Socials = Mapper.Map<IEnumerable<SocialInfoDto>>(socials);
        }

        public ContactDto FindByPrimaryPhone(UserDto user, string phone)
        {
            ContactDto result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContact = contactRepo.FindByPrimaryPhone(user.Id, phone);
                result = Mapper.Map<ContactDto>(dbContact);
                GetPersonalAndSocialsForContact(result, unitOfWork);
            }

            return result;
        }

        public ICollection<ContactDto> GetContacsBySocialInfo(UserDto user, SocialInfoDto info, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var mappedSocial = Mapper.Map<SocialInfo>(info);
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContacts = contactRepo.FilterBySocialInfoAppIdLike(user.Id, mappedSocial, pageSize, page);
                result = Mapper.Map<ICollection<ContactDto>>(dbContacts);
                if (result != null)
                {
                    foreach (var mapped in result)
                    {
                        GetPersonalAndSocialsForContact(mapped, unitOfWork);
                    }
                }
            }

            return result;
        }

        public ContactDto GetContact(int id)
        {
            ContactDto result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContact = contactRepo.GetById(id);
                result = Mapper.Map<ContactDto>(dbContact);
                GetPersonalAndSocialsForContact(result, unitOfWork);
            }

            return result;
        }

        public ICollection<ContactDto> GetContacts(UserDto user, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContacts = contactRepo.GetUserContacts(user.Id, pageSize, page);
                result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                if (result != null)
                {
                    foreach (var mapped in result)
                    {
                        GetPersonalAndSocialsForContact(mapped, unitOfWork);
                    }
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByEmail(UserDto user, string email, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContacts = contactRepo.FilterByEmailStartsWith(user.Id, email, pageSize, page);
                result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                if (result != null)
                {
                    foreach (var mapped in result)
                    {
                        GetPersonalAndSocialsForContact(mapped, unitOfWork);
                    }
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByFirstName(UserDto user, string firstName, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContacts = contactRepo.FilterByFirstNameStartsWith(user.Id, firstName, pageSize, page);
                result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                if (result != null)
                {
                    foreach (var mapped in result)
                    {
                        GetPersonalAndSocialsForContact(mapped, unitOfWork);
                    }
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByLastName(UserDto user, string lastName, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContacts = contactRepo.FilterByLastNameStartsWith(user.Id, lastName, pageSize, page);
                result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                if (result != null)
                {
                    foreach (var mapped in result)
                    {
                        GetPersonalAndSocialsForContact(mapped, unitOfWork);
                    }
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByMiddleName(UserDto user, string middleName, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContacts = contactRepo.FilterByMiddleNameStartsWith(user.Id, middleName, pageSize, page);
                result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                if (result != null)
                {
                    foreach (var mapped in result)
                    {
                        GetPersonalAndSocialsForContact(mapped, unitOfWork);
                    }
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByPersonalInfo(UserDto user, PersonalInfoDto info, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var mappedInfo = Mapper.Map<PersonalInfo>(info);
                var contactRepo = new ContactRepository(unitOfWork);

                var dbContacts = contactRepo.FilterByPersonalInfo(user.Id, mappedInfo, pageSize, page);
                result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                if (result != null)
                {
                    foreach (var mapped in result)
                    {
                        GetPersonalAndSocialsForContact(mapped, unitOfWork);
                    }
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByPhone(UserDto user, string phone, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var social = new SocialInfo()
                {
                    AppName = "Additional Phone",
                    AppId = phone
                };
                var dbContacts = contactRepo.FilterBySocialInfoAppIdLike(user.Id, social, pageSize, page);
                result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                if (result != null)
                {
                    foreach (var mapped in result)
                    {
                        GetPersonalAndSocialsForContact(mapped, unitOfWork);
                    }
                }
            }

            return result;
        }

        public void RemoveContact(ContactDto contact)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                using (var transaction = unitOfWork.BeginTransaction())
                {
                    var contactRepository = new ContactRepository(unitOfWork);
                    var dbContact = contactRepository.GetById(contact.Id);
                    if (dbContact != null && dbContact.PrimaryPhone == contact.PrimaryPhone
                        && dbContact.UserId == contact.UserId)
                    {
                        contactRepository.Delete(contact.Id, transaction);
                        unitOfWork.Commit();
                    }
                    else
                    {
                        throw new Exception("Contact to remove do not exists or not equal to same contact in db.");
                    }
                }
            }
        }
    }
}