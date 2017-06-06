using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Infrastructure
{
    public interface IDbContext : IDisposable
    {
        IUnitOfWork CurrentTransaction { get; }

        bool InTransaction { get; }

        DataSet Users { get; set; }

        DataSet Notes { get; set; }

        DataSet PersonalInfos { get; set; }

        DataSet Contacts { get; set; }

        DataSet Meetings { get; set; }

        DataSet Set(string setName);

        IUnitOfWork BeginTransaction();
    }
}