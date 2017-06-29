using Autofac;
using AutoMapper;
using Organizer.Common.DTO;
using Organizer.Common.Entities;
using Organizer.Common.Enums;
using Organizer.Common.Helpers;
using Organizer.Common.Pagination;
using Organizer.DAL.Helpers;
using Organizer.DAL.Repository;
using Organizer.Infrastructure.Database;
using Organizer.Infrastructure.Services;
using System;
using System.Collections.Generic;

namespace Organizer.BL.Services
{
    public class NoteService : INoteService
    {
        private readonly IContainer _container;

        public NoteService(IContainer container)
        {
            _container = container;
        }

        public void AddNote(NoteDto note)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);
                using (var transaction = unitOfWork.BeginTransaction())
                {
                    var mapped = Mapper.Map<Note>(note);
                    noteRepo.Insert(mapped, transaction);
                    unitOfWork.Commit();
                }
            }
        }

        public void EditNote(NoteDto note)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);
                using (var transaction = unitOfWork.BeginTransaction())
                {
                    var mapped = Mapper.Map<Note>(note);
                    noteRepo.Update(mapped, transaction);
                    unitOfWork.Commit();
                }
            }
        }

        public NoteDto GetNote(int noteId)
        {
            NoteDto result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNote = noteRepo.GetById(noteId);
                result = Mapper.Map<NoteDto>(dbNote);
            }

            return result;
        }

        public NoteDto GetNoteByCaption(UserDto user, string caption)
        {
            NoteDto result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNote = noteRepo.GetNoteByCaption(user.Id, caption);
                result = Mapper.Map<NoteDto>(dbNote);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotes(UserDto user, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetUserNotesQuery(),
                    NoteParams.GetGetUserNotesParams(user.Id));

                var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                var dbNotes = noteRepo.GetUserNotes(user.Id, temp.PageSize, temp.PageNumber);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByCaptionLike(UserDto user, string caption, NoteType noteType, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByCaptionQuery(),
                    NoteParams.GetFilterByCaptionParams(user.Id, caption, noteType));

                var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                var dbNotes = noteRepo.FilterByCaptionLike(user.Id, caption, noteType, temp.PageSize, temp.PageNumber);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByCreationDate(UserDto user, DateTime creationDate, NoteType noteType, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByCreationDateQuery(),
                    NoteParams.GetFilterByCreationDateParams(user.Id, creationDate, noteType));

                var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                var dbNotes = noteRepo.FilterByCreationDate(user.Id, creationDate, noteType, temp.PageSize, temp.PageNumber);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByCurrentState(UserDto user, State state, NoteType noteType, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByStateQuery(),
                    NoteParams.GetFilterByStateParams(user.Id, state, noteType));

                var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                var dbNotes = noteRepo.FilterByCurrentState(user.Id, state, noteType, temp.PageSize, temp.PageNumber);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByEndDate(UserDto user, DateTime endDate, NoteType noteType, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByEndDateQuery(),
                    NoteParams.GetFilterByEndDateParams(user.Id, endDate, noteType));

                var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                var dbNotes = noteRepo.FilterByEndDate(user.Id, endDate, noteType, temp.PageSize, temp.PageNumber);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByLastChangeDate(UserDto user, DateTime lastChangeDate, NoteType noteType, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetUserNotesQuery(),
                    NoteParams.GetGetUserNotesParams(user.Id));

                var temp = PaginationHelper
                    .CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                var dbNotes = noteRepo.FilterByLastChangeDate(user.Id, lastChangeDate, noteType, temp.PageSize, temp.PageNumber);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByNoteType(UserDto user, NoteType noteType, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByNoteType(user.Id, noteType, pageSize, page);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByPriority(UserDto user, Priority priority, NoteType noteType, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByPriorityQuery(),
                    NoteParams.GetFilterByPriorityParams(user.Id, priority, noteType));

                var temp = PaginationHelper
                    .CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                var dbNotes = noteRepo.FilterByPriority(user.Id, priority, noteType, temp.PageSize, temp.PageNumber);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByStartDate(UserDto user, DateTime startDate, NoteType noteType, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByStartDateQuery(),
                    NoteParams.GetFilterByStartDateParams(user.Id, startDate, noteType));

                var temp = PaginationHelper
                    .CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                var dbNotes = noteRepo.FilterByStartDate(user.Id, startDate, noteType, temp.PageSize, temp.PageNumber);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesCreatedBetween(UserDto user, DateTime start, DateTime end, NoteType noteType, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByCreationBetweenQuery(),
                    NoteParams.GetFilterByCreationBetweenParams(user.Id, start, end, noteType));

                var temp = PaginationHelper
                    .CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                var dbNotes = noteRepo.FilterByCreationBetween(user.Id, start, end, noteType, temp.PageSize, temp.PageNumber);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public void RemoveNote(NoteDto note)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                using (var transaction = unitOfWork.BeginTransaction())
                {
                    var noteRepository = new NoteRepository(unitOfWork);
                    var dbNote = noteRepository.GetById(note.Id);
                    if (dbNote != null && dbNote.Caption == note.Caption
                        && dbNote.UserId == note.UserId)
                    {
                        noteRepository.Delete(note.Id, transaction);
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