using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Entities;

namespace ToDo.Domain.Repositories.Base
{
    public interface IWriteOnlyRepository<TEntity> : IRepository
        where TEntity : AggregateRoot
    {
        public void Create(TEntity entity);
        public void CreateRange(IEnumerable<TEntity> entities);
        public void Update(TEntity entity);
        public void UpdateRange(IEnumerable<TEntity> entities);
        public void Delete(TEntity entity);
        public void DeleteRange(IEnumerable<TEntity> entities);
    }
}
