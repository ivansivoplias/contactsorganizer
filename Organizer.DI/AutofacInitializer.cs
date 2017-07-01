using Autofac;
using Organizer.BL.Services;
using Organizer.Common.Entities;
using Organizer.DAL.Context;
using Organizer.DAL.Repository;
using Organizer.DAL.UoW;
using Organizer.Infrastructure.Database;
using Organizer.Infrastructure.Services;
using System;

namespace Organizer.DI
{
    public static class AutofacInitializer
    {
        public static void Initialize(ContainerBuilder builder, string connectionString, Func<IContainer> factory)
        {
            builder.Register(x => factory());

            builder.Register(x => new DatabaseContextFactory(connectionString))
                .As<IDatabaseContextFactory>();

            builder.Register(x =>
            {
                var contextFactory = x.Resolve<IDatabaseContextFactory>();
                return new UnitOfWork(contextFactory);
            }).As<IUnitOfWork>();

            builder.Register(x =>
            {
                var t = x.Resolve<IUnitOfWork>();
                return new MeetingRepository(t);
            }).As<IMeetingRepository>();

            builder.Register(x =>
            {
                var t = x.Resolve<IUnitOfWork>();
                return new NoteRepository(t);
            }).As<INoteRepository>();

            builder.Register(x =>
            {
                var t = x.Resolve<IUnitOfWork>();
                return new ContactRepository(t);
            }).As<IContactRepository>();

            builder.Register(x =>
            {
                var t = x.Resolve<IUnitOfWork>();
                return new UserRepository(t);
            }).As<IRepository<User>>();

            builder.Register(x =>
            {
                var t = x.Resolve<IUnitOfWork>();
                return new PersonalInfoRepository(t);
            }).As<IRepository<PersonalInfo>>();

            builder.Register(x =>
            {
                var t = x.Resolve<IContainer>();
                return new ContactService(t);
            }).As<IContactService>();

            builder.Register(x =>
            {
                var t = x.Resolve<IContainer>();
                return new UserService(t);
            }).As<IUserService>();

            builder.Register(x =>
            {
                var t = x.Resolve<IContainer>();
                return new MeetingService(t);
            }).As<IMeetingService>();

            builder.Register(x =>
            {
                var t = x.Resolve<IContainer>();
                return new NoteService(t);
            }).As<INoteService>();
        }
    }
}