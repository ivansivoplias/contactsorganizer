using Autofac;
using Organizer.Common.Entities;
using Organizer.Common.Enums;
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
    public class NoteService : INoteService
    {
        private readonly IContainer _container;

        public NoteService(IContainer container)
        {
            _container = container;
        }

        public void AddNote(Note note)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNote = noteRepo.GetNoteByCaption(note.UserId, note.NoteType, note.Caption);

                if (dbNote == null)
                {
                    using (var transaction = unitOfWork.BeginTransaction())
                    {
                        noteRepo.Insert(note, transaction);
                        unitOfWork.Commit();
                    }
                }
                else
                {
                    throw new NoteCaptionAlreadyExistsException($"{note.NoteType} with caption: {note.Caption} already exists in database.");
                }
            }
        }

        public void EditNote(Note note)
        {
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FindNotesByCaption(note.UserId, note.Caption, note.NoteType).ToList();

                if (dbNotes.Count == 0 || (dbNotes.Count == 1 && dbNotes[0].Id == note.Id))
                {
                    using (var transaction = unitOfWork.BeginTransaction())
                    {
                        noteRepo.Update(note, transaction);
                        unitOfWork.Commit();
                    }
                }
                else
                {
                    throw new NoteCaptionAlreadyExistsException($"{note.NoteType} with caption: {note.Caption} already exists in database.");
                }
            }
        }

        public Note GetNote(int noteId)
        {
            Note result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                result = noteRepo.GetById(noteId);
            }

            return result;
        }

        public Note GetNoteByCaption(User user, NoteType noteType, string caption)
        {
            Note result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                result = noteRepo.GetNoteByCaption(user.Id, noteType, caption);
            }

            return result;
        }

        public ICollection<Note> GetNotes(User user, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetUserNotesQuery(),
                    NoteParams.GetGetUserNotesParams(user.Id));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = noteRepo.GetUserNotes(user.Id, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Note>();
                }
            }

            return result;
        }

        public ICollection<Note> GetNotesByCaptionLike(User user, string caption, NoteType noteType, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByCaptionQuery(),
                    NoteParams.GetFilterByCaptionParams(user.Id, caption, noteType));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = noteRepo.FilterByCaptionLike(user.Id, caption, noteType, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Note>();
                }
            }

            return result;
        }

        public ICollection<Note> GetNotesByCreationDate(User user, DateTime creationDate, NoteType noteType, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByCreationDateQuery(),
                    NoteParams.GetFilterByCreationDateParams(user.Id, creationDate, noteType));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = noteRepo.FilterByCreationDate(user.Id, creationDate, noteType, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Note>();
                }
            }

            return result;
        }

        public ICollection<Note> GetNotesByCurrentState(User user, State state, NoteType noteType, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByStateQuery(),
                    NoteParams.GetFilterByStateParams(user.Id, state, noteType));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = noteRepo.FilterByCurrentState(user.Id, state, noteType, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Note>();
                }
            }

            return result;
        }

        public ICollection<Note> GetNotesByEndDate(User user, DateTime endDate, NoteType noteType, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByEndDateQuery(),
                    NoteParams.GetFilterByEndDateParams(user.Id, endDate, noteType));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper.CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = noteRepo.FilterByEndDate(user.Id, endDate, noteType, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Note>();
                }
            }

            return result;
        }

        public ICollection<Note> GetNotesByLastChangeDate(User user, DateTime lastChangeDate, NoteType noteType, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetUserNotesQuery(),
                    NoteParams.GetGetUserNotesParams(user.Id));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper
                        .CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = noteRepo.FilterByLastChangeDate(user.Id, lastChangeDate, noteType, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Note>();
                }
            }

            return result;
        }

        public ICollection<Note> GetNotesByNoteType(User user, NoteType noteType, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                result = noteRepo.FilterByNoteType(user.Id, noteType, pageSize, page)
                    .ToList();
            }

            return result;
        }

        public ICollection<Note> GetNotesByPriority(User user, Priority priority, NoteType noteType, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByPriorityQuery(),
                    NoteParams.GetFilterByPriorityParams(user.Id, priority, noteType));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper
                        .CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = noteRepo.FilterByPriority(user.Id, priority, noteType, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Note>();
                }
            }

            return result;
        }

        public ICollection<Note> GetNotesByStartDate(User user, DateTime startDate, NoteType noteType, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByStartDateQuery(),
                    NoteParams.GetFilterByStartDateParams(user.Id, startDate, noteType));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper
                        .CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = noteRepo.FilterByStartDate(user.Id, startDate, noteType, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Note>();
                }
            }

            return result;
        }

        public ICollection<Note> GetNotesCreatedBetween(User user, DateTime start, DateTime end, NoteType noteType, int pageSize, int page)
        {
            ICollection<Note> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var filteredCount = noteRepo.FilteredCount(NoteQueries.GetFilterByCreationBetweenQuery(),
                    NoteParams.GetFilterByCreationBetweenParams(user.Id, start, end, noteType));

                if (filteredCount > 0)
                {
                    var temp = PaginationHelper
                        .CheckPaginationAndAdoptValues(new Page(filteredCount, page, pageSize));

                    result = noteRepo.FilterByCreationBetween(user.Id, start, end, noteType, temp.PageSize, temp.PageNumber)
                        .ToList();
                }
                else
                {
                    result = new List<Note>();
                }
            }

            return result;
        }

        public void RemoveNote(Note note)
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