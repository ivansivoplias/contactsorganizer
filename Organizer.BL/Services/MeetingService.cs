using Autofac;
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
    public class MeetingService : IMeetingService
    {
        private readonly IContainer _container;

        public MeetingService(IContainer container)
        {
            _container = container;
        }

        public void AddMeeting(Meeting meeting)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var dbMeeting = meetingRepo.FindByMeetingName(meeting.UserId, meeting.MeetingName);

                if (dbMeeting == null)
                {
                    unitOfWork.BeginTransaction();

                    meetingRepo.Insert(meeting);

                    unitOfWork.Commit();
                }
                else
                {
                    throw new MeetingNameAlreadyExistsException($"Meeting with name: {meeting.MeetingName} already exists in database.");
                }
            }
        }

        public void EditMeeting(Meeting meeting)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var dbMeeting = meetingRepo.FindByMeetingName(meeting.UserId, meeting.MeetingName);

                if (dbMeeting == null || dbMeeting.Id == meeting.Id)
                {
                    unitOfWork.BeginTransaction();
                    meetingRepo.Update(meeting);
                    unitOfWork.Commit();
                }
                else
                {
                    throw new MeetingNameAlreadyExistsException($"The other meeting with name: {meeting.MeetingName} already exists in database.");
                }
            }
        }

        public ICollection<Meeting> FilterByMeetingDate(User user, DateTime meetingDate, int pageSize, int page)
        {
            ICollection<Meeting> result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var filteredCount = meetingRepo.FilteredCount(MeetingQueries.GetFilterByMeetingDateQuery(),
                    MeetingParams.GetFilterByMeetingDateParams(user.Id, meetingDate));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = meetingRepo.FilterByMeetingDate(user.Id, meetingDate, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Meeting>();
                }
            }

            return result;
        }

        public ICollection<Meeting> FilterByMeetingName(User user, string meetingName, int pageSize, int page)
        {
            ICollection<Meeting> result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var filteredCount = meetingRepo.FilteredCount(MeetingQueries.GetFilterByMeetingName(),
                    MeetingParams.GetFilterByMeetingNameParams(user.Id, meetingName));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = meetingRepo.FilterByMeetingNameLike(user.Id, meetingName, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Meeting>();
                }
            }

            return result;
        }

        public int GetFilterByMeetingDateCount(User user, DateTime meetingDate)
        {
            int count = 0;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                count = meetingRepo.FilteredCount(MeetingQueries.GetFilterByMeetingDateQuery(),
                    MeetingParams.GetFilterByMeetingDateParams(user.Id, meetingDate));
                if (count < 0)
                    count = 0;
            }
            return count;
        }

        public int GetFilterByMeetingNameCount(User user, string meetingName)
        {
            int count = 0;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                count = meetingRepo.FilteredCount(MeetingQueries.GetFilterByMeetingName(),
                    MeetingParams.GetFilterByMeetingNameParams(user.Id, meetingName));
                if (count < 0)
                    count = 0;
            }
            return count;
        }

        public Meeting GetMeeting(int meetingId)
        {
            Meeting result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                result = meetingRepo.GetById(meetingId);
            }

            return result;
        }

        public Meeting GetMeetingByName(int userId, string meetingName)
        {
            Meeting result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                result = meetingRepo.FindByMeetingName(userId, meetingName);
            }

            return result;
        }

        public int GetMeetingsCount(User user)
        {
            int count = 0;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                count = meetingRepo.FilteredCount(MeetingQueries.GetUserMeetingsQuery(),
                    MeetingParams.GetGetUserMeetingsParams(user.Id));
                if (count < 0)
                    count = 0;
            }
            return count;
        }

        public ICollection<Meeting> GetUserMeetings(User user, int pageSize, int page)
        {
            ICollection<Meeting> result = null;

            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var meetingRepo = new MeetingRepository(unitOfWork);

                var filteredCount = meetingRepo.FilteredCount(MeetingQueries.GetUserMeetingsQuery(),
                    MeetingParams.GetGetUserMeetingsParams(user.Id));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = meetingRepo.GetUserMeetings(user.Id, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Meeting>();
                }
            }

            return result;
        }

        public void RemoveMeeting(Meeting meeting)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                unitOfWork.BeginTransaction();
                var contactRepository = new MeetingRepository(unitOfWork);
                var dbMeeting = contactRepository.GetById(meeting.Id);
                if (dbMeeting != null && dbMeeting.MeetingName == meeting.MeetingName
                    && dbMeeting.UserId == meeting.UserId)
                {
                    contactRepository.Delete(meeting.Id);
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