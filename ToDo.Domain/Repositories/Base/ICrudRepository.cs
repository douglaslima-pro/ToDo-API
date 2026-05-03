using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Entities;

namespace ToDo.Domain.Repositories.Base
{
    public interface ICrudRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>, IWriteOnlyRepository<TEntity>
        where TEntity : AggregateRoot
    {

    }
}
