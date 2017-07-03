using Autofac;
using AutoMapper;
using Organizer.Common.DTO;
using Organizer.Common.Entities;
using Organizer.Common.Exceptions;
using Organizer.Common.Helpers;
using Organizer.Common.Pagination;
using Organizer.DAL.Helpers;
using Organizer.DAL.Repository;
using Organizer.Infrastructure.Database;
using Organizer.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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

                var dbContact = contactsRepo.FindByPrimaryPhone(contact.UserId, contact.PrimaryPhone);

                if (dbContact == null)
                {
                    using (var transaction = unitOfWork.BeginTransaction())
                    {
                        contactsRepo.Insert(mappedContact, transaction);

                        var added = contactsRepo.FindByPrimaryPhone(contact.UserId, contact.PrimaryPhone);

                        AddPersonalInfo(added.Id, contact.PersonalInfo, unitOfWork);
                        AddSocials(added.Id, contact.Socials, unitOfWork);
                        unitOfWork.Commit();
                    }
                }
                else
                {
                    throw new PrimaryPhoneAlreadyExistException($"Contact with primary phone: {contact.PrimaryPhone} already exists in database.");
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

                var dbContacts = contactsRepo.FindContactsByPrimaryPhone(contact.UserId, contact.PrimaryPhone).ToList();

                if (dbContacts.Count == 0 || (dbContacts.Count == 1 && dbContacts[0].Id == contact.Id))
                {
                    using (var transaction = unitOfWork.BeginTransaction())
                    {
                        contactsRepo.Update(mappedContact, transaction);
                        EditPersonalInfo(contact.PersonalInfo, unitOfWork);
                        EditSocials(contact.Id, contact.Socials, unitOfWork);
                        unitOfWork.Commit();
                    }
                }
                else
                {
                    throw new PrimaryPhoneAlreadyExistException($"Contact with primary phone: {contact.PrimaryPhone} already exists in database.");
                }
            }
        }

        private void AddPersonalInfo(int contactId, PersonalInfo personalInfo, IUnitOfWork unitOfWork)
        {
            var personalRepo = new PersonalInfoRepository(unitOfWork);
            personalInfo.Id = contactId;
            personalRepo.Insert(personalInfo, unitOfWork.Transaction);
        }

        private void AddSocials(int contactId, IEnumerable<SocialInfo> socials, IUnitOfWork unitOfWork)
        {
            var socialRepo = new SocialInfoRepository(unitOfWork);
            foreach (var social in socials)
            {
                social.ContactId = contactId;
                socialRepo.Insert(social, unitOfWork.Transaction);
            }
        }

        private void EditPersonalInfo(PersonalInfo personalInfo, IUnitOfWork unitOfWork)
        {
            var personalRepo = new PersonalInfoRepository(unitOfWork);
            personalRepo.Update(personalInfo, unitOfWork.Transaction);
        }

        private void EditSocials(int contactId, IEnumerable<SocialInfo> socials, IUnitOfWork unitOfWork)
        {
            var socialRepo = new SocialInfoRepository(unitOfWork);

            var dbSocials = socialRepo.GetContactSocials(contactId);

            foreach (var socialInfo in socials)
            {
                var dbSocial = dbSocials.FirstOrDefault(x => x.Id == (socialInfo.Id));
                if (dbSocial != null)
                {
                    socialRepo.Update(socialInfo, unitOfWork.Transaction);
                }
                else
                {
                    socialInfo.ContactId = contactId;
                    socialRepo.Insert(socialInfo, unitOfWork.Transaction);
                }
            }

            var deleted = dbSocials.Where(x => socials.FirstOrDefault(y => y.Id == x.Id) == null);
            foreach (var del in deleted)
            {
                socialRepo.Delete(del.Id, unitOfWork.Transaction);
            }
        }

        public ContactDto FindByNickName(User user, string nickName)
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

            contact.PersonalInfo = personalRepo.GetById(contact.Id);

            contact.Socials = socialRepo.GetContactSocials(contact.Id);
        }

        public ContactDto FindByPrimaryPhone(User user, string phone)
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

        public ICollection<ContactDto> GetContacsBySocialInfo(User user, string appId, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var filteredCount = contactRepo.FilteredCount(ContactQueries.GetFilterBySocialInfoQuery(),
                    ContactParams.GetFilterBySocialInfoParams(user.Id, appId));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper
                        .CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    var dbContacts = contactRepo
                        .FilterBySocialInfoAppIdLike(user.Id, appId, temp.PageSize, temp.PageNumber);
                    result = Mapper.Map<ICollection<ContactDto>>(dbContacts);
                    if (result != null)
                    {
                        foreach (var mapped in result)
                        {
                            GetPersonalAndSocialsForContact(mapped, unitOfWork);
                        }
                    }
                }
                else
                {
                    result = new List<ContactDto>();
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

        public ICollection<ContactDto> GetContacts(User user, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var filteredCount = contactRepo.FilteredCount(ContactQueries.GetUserContactsQuery(),
                    ContactParams.GetGetUserContactsParams(user.Id));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    var dbContacts = contactRepo.GetUserContacts(user.Id, temp.PageSize, temp.PageNumber);
                    result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                    if (result != null)
                    {
                        foreach (var mapped in result)
                        {
                            GetPersonalAndSocialsForContact(mapped, unitOfWork);
                        }
                    }
                }
                else
                {
                    result = new List<ContactDto>();
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByEmail(User user, string email, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var filteredCount = contactRepo.FilteredCount(ContactQueries.GetFilterByEmailLikeQuery(),
                    ContactParams.GetFilterByEmailParams(user.Id, email));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    var dbContacts = contactRepo.FilterByEmailStartsWith(user.Id, email, temp.PageSize, temp.PageNumber);
                    result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                    if (result != null)
                    {
                        foreach (var mapped in result)
                        {
                            GetPersonalAndSocialsForContact(mapped, unitOfWork);
                        }
                    }
                }
                else
                {
                    result = new List<ContactDto>();
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByFirstName(User user, string firstName, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var filteredCount = contactRepo.FilteredCount(ContactQueries.GetFilterByFirstNameQuery(),
                    ContactParams.GetFilterByFirstNameParams(user.Id, firstName));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    var dbContacts = contactRepo.FilterByFirstNameStartsWith(user.Id, firstName, temp.PageSize, temp.PageNumber);
                    result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                    if (result != null)
                    {
                        foreach (var mapped in result)
                        {
                            GetPersonalAndSocialsForContact(mapped, unitOfWork);
                        }
                    }
                }
                else
                {
                    result = new List<ContactDto>();
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByLastName(User user, string lastName, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var filteredCount = contactRepo.FilteredCount(ContactQueries.GetFilterByLastNameQuery(),
                    ContactParams.GetFilterByLastnameParams(user.Id, lastName));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    var dbContacts = contactRepo.FilterByLastNameStartsWith(user.Id, lastName, temp.PageSize, temp.PageNumber);
                    result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                    if (result != null)
                    {
                        foreach (var mapped in result)
                        {
                            GetPersonalAndSocialsForContact(mapped, unitOfWork);
                        }
                    }
                }
                else
                {
                    result = new List<ContactDto>();
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByMiddleName(User user, string middleName, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var filteredCount = contactRepo.FilteredCount(ContactQueries.GetFilterByMiddleNameQuery(),
                    ContactParams.GetFilterByMiddleNameParams(user.Id, middleName));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    var dbContacts = contactRepo.FilterByMiddleNameStartsWith(user.Id, middleName, temp.PageSize, temp.PageNumber);
                    result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                    if (result != null)
                    {
                        foreach (var mapped in result)
                        {
                            GetPersonalAndSocialsForContact(mapped, unitOfWork);
                        }
                    }
                }
                else
                {
                    result = new List<ContactDto>();
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByPersonalInfo(User user, string personalInfo, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var filteredCount = contactRepo.FilteredCount(ContactQueries.GetFilterByPersonalInfoQuery(),
                    ContactParams.GetFilterByPersonalInfoParams(user.Id, personalInfo));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    var dbContacts = contactRepo.FilterByPersonalInfo(user.Id, personalInfo, temp.PageSize, temp.PageNumber);
                    result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                    if (result != null)
                    {
                        foreach (var mapped in result)
                        {
                            GetPersonalAndSocialsForContact(mapped, unitOfWork);
                        }
                    }
                }
                else
                {
                    result = new List<ContactDto>();
                }
            }

            return result;
        }

        public ICollection<ContactDto> GetContactsByPhone(User user, string phone, int pageSize, int page)
        {
            ICollection<ContactDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var contactRepo = new ContactRepository(unitOfWork);

                var social = new SocialInfo()
                {
                    AppName = "Phone",
                    AppId = phone
                };

                var filteredCount = contactRepo.FilteredCount(ContactQueries.GetFilterByAppInfoLikeQuery(),
                    ContactParams.GetFilterByAppInfoLikeParams(user.Id, social));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    var dbContacts = contactRepo.FilterByAppInfoLike(user.Id, social, temp.PageSize, temp.PageNumber);
                    result = Mapper.Map<ICollection<ContactDto>>(dbContacts);

                    if (result != null)
                    {
                        foreach (var mapped in result)
                        {
                            GetPersonalAndSocialsForContact(mapped, unitOfWork);
                        }
                    }
                }
                else
                {
                    result = new List<ContactDto>();
                }

                var contact = contactRepo.FindByPrimaryPhone(user.Id, phone);

                if (contact != null)
                {
                    var mapped = Mapper.Map<ContactDto>(contact);
                    GetPersonalAndSocialsForContact(mapped, unitOfWork);
                    result.Add(mapped);
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