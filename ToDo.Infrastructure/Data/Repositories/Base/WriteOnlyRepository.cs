using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Domain.Repositories.Base;

namespace ToDo.Infrastructure.Data.Repositories.Base
{
    public abstract class WriteOnlyRepository<TContext, TEntity, TKey> : Repository<TContext>, IWriteOnlyRepository<TEntity>
        where TContext : DbContext
        where TEntity : AggregateRoot
    {
        protected readonly DbSet<TEntity> _entity;
        
        protected WriteOnlyRepository(TContext context) : base(context)
        {
            _entity = _context.Set<TEntity>();
        }

        public void Create(TEntity entity) => _entity.Add(entity);

        public void CreateRange(IEnumerable<TEntity> entities) => _entity.AddRange(entities);

        public void Update(TEntity entity) => _entity.Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities) => _entity.UpdateRange(entities);

        public void Delete(TEntity entity) => _entity.Remove(entity);

        public void DeleteRange(IEnumerable<TEntity> entities) => _entity.RemoveRange(entities);
    }
}
