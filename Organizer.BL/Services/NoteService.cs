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

        public ICollection<NoteDto> GetNotes(UserDto user)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.GetUserNotes(user.Id);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByCreationDate(UserDto user, DateTime creationDate)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByCreationDate(user.Id, creationDate);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByCurrentState(UserDto user, State state)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByCurrentState(user.Id, state);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByEndDate(UserDto user, DateTime endDate)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByEndDate(user.Id, endDate);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByLastChangeDate(UserDto user, DateTime lastChangeDate)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByLastChangeDate(user.Id, lastChangeDate);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByNoteType(UserDto user, NoteType noteType)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByNoteType(user.Id, noteType);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByPriority(UserDto user, Priority priority)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByPriority(user.Id, priority);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesByStartDate(UserDto user, DateTime startDate)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByStartDate(user.Id, startDate);
                result = Mapper.Map<ICollection<NoteDto>>(dbNotes);
            }

            return result;
        }

        public ICollection<NoteDto> GetNotesCreatedBetween(UserDto user, DateTime start, DateTime end)
        {
            ICollection<NoteDto> result = null;
            var unitOfWork = _container.Resolve<IUnitOfWork>();
            using (unitOfWork)
            {
                var noteRepo = new NoteRepository(unitOfWork);

                var dbNotes = noteRepo.FilterByCreationBetween(user.Id, start, end);
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