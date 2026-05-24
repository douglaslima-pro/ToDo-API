using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Domain.Repositories.Base;

namespace ToDo.Infrastructure.Data.Repositories.Base
{
    public abstract class CrudRepository<TContext, TEntity, TKey> : Repository<TContext>, ICrudRepository<TEntity, TKey>
        where TContext : DbContext
        where TEntity : AggregateRoot
    {
        protected readonly DbSet<TEntity> _entity;

        protected CrudRepository(TContext context) : base(context)
        {
            _entity = _context.Set<TEntity>();
        }

        // Read
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? searchExpression = null)
        {
            if (searchExpression != null)
            {
                return await _entity.CountAsync(searchExpression);
            }
            else
            {
                return await _entity.CountAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? searchExpression = null)
        {
            if (searchExpression != null)
            {
                return await _entity.AnyAsync(searchExpression);
            }
            else
            {
                return await _entity.AnyAsync();
            }
        }

        public async Task<TEntity?> GetByIdAsync(TKey id) => await _entity.FindAsync(id);

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? searchExpression = null, Expression<Func<TEntity, object>>? orderByExpression = null, string[]? includeExpression = null, bool isAscending = true)
        {
            IQueryable<TEntity> query = _entity.AsQueryable();

            if (searchExpression != null)
            {
                query = query.Where(searchExpression);
            }

            if (orderByExpression != null)
            {
                if (isAscending)
                {
                    query.OrderBy(orderByExpression);
                }
                else
                {
                    query.OrderByDescending(orderByExpression);
                }
            }

            if (includeExpression != null)
            {
                foreach (var include in includeExpression)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? searchExpression = null, Expression<Func<TEntity, object>>? orderByExpression = null, string[]? includeExpression = null, bool isAscending = true)
        {
            IQueryable<TEntity> query = _entity.AsQueryable();

            if (searchExpression != null)
            {
                query = query.Where(searchExpression);
            }

            if (orderByExpression != null)
            {
                if (isAscending)
                {
                    query.OrderBy(orderByExpression);
                }
                else
                {
                    query.OrderByDescending(orderByExpression);
                }
            }

            if (includeExpression != null)
            {
                foreach (var include in includeExpression)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>>? searchExpression = null, Expression<Func<TEntity, object>>? orderByExpression = null, string[]? includeExpression = null, bool isAscending = true, int start = 0, int length = 10)
        {
            IQueryable<TEntity> query = _entity.AsQueryable();

            if (searchExpression != null)
            {
                query = query.Where(searchExpression);
            }

            if (orderByExpression != null)
            {
                if (isAscending)
                {
                    query.OrderBy(orderByExpression);
                }
                else
                {
                    query.OrderByDescending(orderByExpression);
                }
            }

            if (includeExpression != null)
            {
                foreach (var include in includeExpression)
                {
                    query = query.Include(include);
                }
            }

            return await query
                .Skip(start)
                .Take(length)
                .ToListAsync();
        }

        // Write
        public void Create(TEntity entity) => _entity.Add(entity);

        public void CreateRange(IEnumerable<TEntity> entities) => _entity.AddRange(entities);

        public void Update(TEntity entity) => _entity.Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities) => _entity.UpdateRange(entities);

        public void Delete(TEntity entity) => _entity.Remove(entity);

        public void DeleteRange(IEnumerable<TEntity> entities) => _entity.RemoveRange(entities);
    }
}
