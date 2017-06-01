using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Infrastructure
{
    public interface IRepositoryProvider
    {
        IRepository<TEntity> GetRepositoryForKey<TEntity>(string typeKey) where TEntity : IEntity;
    }
}