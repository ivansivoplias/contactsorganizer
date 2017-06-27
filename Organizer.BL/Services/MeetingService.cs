using Organizer.Infrastructure.Services;
using System;
using System.Collections.Generic;
using Organizer.Common.DTO;
using Autofac;
using Organizer.Infrastructure.Database;
using Organizer.DAL.Repository;
using Organizer.Common.Entities;
using AutoMapper;

namespace Organizer.BL.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IContainer _container;

        public MeetingService(IContainer container)
        {
            _container = container;
        }

        public void AddMeeting(MeetingDto meeting)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);
                using (var transaction = unitOfWork.BeginTransaction())
                {
                    var mapped = Mapper.Map<Meeting>(meeting);
                    meetingRepo.Insert(mapped, transaction);
                    unitOfWork.Commit();
                }
            }
        }

        public void EditMeeting(MeetingDto meeting)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);
                using (var transaction = unitOfWork.BeginTransaction())
                {
                    var mapped = Mapper.Map<Meeting>(meeting);
                    meetingRepo.Update(mapped, transaction);
                    unitOfWork.Commit();
                }
            }
        }

        public ICollection<MeetingDto> FilterByMeetingDate(UserDto user, DateTime meetingDate, int pageSize, int page)
        {
            ICollection<MeetingDto> result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var meetings = meetingRepo.FilterByMeetingDate(user.Id, meetingDate, pageSize, page);
                result = Mapper.Map<ICollection<MeetingDto>>(meetings);
            }

            return result;
        }

        public ICollection<MeetingDto> FilterByMeetingName(UserDto user, string meetingName, int pageSize, int page)
        {
            ICollection<MeetingDto> result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var meetings = meetingRepo.FilterByMeetingNameLike(user.Id, meetingName, pageSize, page);
                result = Mapper.Map<ICollection<MeetingDto>>(meetings);
            }

            return result;
        }

        public MeetingDto GetMeeting(int meetingId)
        {
            MeetingDto result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var meeting = meetingRepo.GetById(meetingId);
                result = Mapper.Map<MeetingDto>(meeting);
            }

            return result;
        }

        public MeetingDto GetMeetingByName(string meetingName)
        {
            MeetingDto result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var meeting = meetingRepo.FindByMeetingName(meetingName);
                result = Mapper.Map<MeetingDto>(meeting);
            }

            return result;
        }

        public ICollection<MeetingDto> GetUserMeetings(UserDto user, int pageSize, int page)
        {
            ICollection<MeetingDto> result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var meetings = meetingRepo.GetUserMeetings(user.Id, pageSize, page);
                result = Mapper.Map<ICollection<MeetingDto>>(meetings);
            }

            return result;
        }

        public void RemoveMeeting(MeetingDto meeting)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                using (var transaction = unitOfWork.BeginTransaction())
                {
                    var contactRepository = new MeetingRepository(unitOfWork);
                    var dbMeeting = contactRepository.GetById(meeting.Id);
                    if (dbMeeting != null && dbMeeting.MeetingName == meeting.MeetingName
                        && dbMeeting.UserId == meeting.UserId)
                    {
                        contactRepository.Delete(meeting.Id, transaction);
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