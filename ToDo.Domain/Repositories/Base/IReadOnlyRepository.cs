using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Entities;

namespace ToDo.Domain.Repositories.Base
{
    public interface IReadOnlyRepository<TEntity, TKey> : IRepository
        where TEntity : AggregateRoot
    {
        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? searchExpression = null);
        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? searchExpression = null);
        public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? searchExpression = null, Expression<Func<TEntity, object>>? orderByExpression = null, bool isAscending = true);
        public Task<TEntity?> GetByIdAsync(TKey id);
        public Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? searchExpression = null, Expression<Func<TEntity, object>>? orderByExpression = null, bool isAscending = true);
        public Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>>? searchExpression = null, Expression<Func<TEntity, object>>? orderByExpression = null, bool isAscending = true, int start = 0, int length = 10);
    }
}
