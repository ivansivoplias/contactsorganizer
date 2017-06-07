using System;
using System.Data;

namespace Organizer.Infrastructure
{
    public interface IDbContext : IDisposable
    {
        DataSet Users { get; set; }

        DataSet Notes { get; set; }

        DataSet PersonalInfos { get; set; }

        DataSet Contacts { get; set; }

        DataSet Meetings { get; set; }

        DataSet Set(string setName);

        void SaveChanges();
    }
}