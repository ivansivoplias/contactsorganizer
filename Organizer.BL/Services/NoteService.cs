using Organizer.Infrastructure.Services;
using System;
using System.Collections.Generic;
using Organizer.Common.DTO;
using Organizer.Common.Enums;
using Autofac;
using Organizer.Infrastructure.Database;
using Organizer.DAL.Repository;
using Organizer.Common.Entities;
using AutoMapper;

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

                var dbNotes = noteRepo.GetUserNotes(user.Id, pageSize, page);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByCreationDate(UserDto user, DateTime creationDate, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByCreationDate(user.Id, creationDate, pageSize, page);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByCurrentState(UserDto user, State state, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByCurrentState(user.Id, state, pageSize, page);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByEndDate(UserDto user, DateTime endDate, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByEndDate(user.Id, endDate, pageSize, page);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByLastChangeDate(UserDto user, DateTime lastChangeDate, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByLastChangeDate(user.Id, lastChangeDate, pageSize, page);
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

        public ICollection<NoteDto> GetNotesByPriority(UserDto user, Priority priority, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByPriority(user.Id, priority, pageSize, page);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByStartDate(UserDto user, DateTime startDate, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByStartDate(user.Id, startDate, pageSize, page);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesCreatedBetween(UserDto user, DateTime start, DateTime end, int pageSize, int page)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByCreationBetween(user.Id, start, end, pageSize, page);
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