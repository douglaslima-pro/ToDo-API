using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities.TaskLists;
using ToDo.Domain.Repositories;
using ToDo.Infrastructure.Data.Contexts;
using ToDo.Infrastructure.Data.Repositories.Base;

namespace ToDo.Infrastructure.Data.Repositories
{
    public class TaskListRepository : CrudRepository<ToDoDBContext, TaskList, int>, ITaskListRepository
    {
        public TaskListRepository(ToDoDBContext context) : base(context) { }

        public async Task<IEnumerable<TaskListItem>> GetAllTasksFromListAsync(int taskListId, Expression<Func<TaskListItem, object>>? orderByExpression = null, bool isAscending = true, int start = 0, int length = 10)
        {
            IQueryable<TaskListItem> query = _context
                .Set<TaskListItem>()
                .AsQueryable();

            query = query.Where(t => t.Id == taskListId);

            if (orderByExpression != null)
            {
                if (isAscending)
                {
                    query = query
                        .OrderBy(orderByExpression);
                }
                else
                {
                    query = query
                        .OrderByDescending(orderByExpression);
                }
            }

            return await query
                .Skip(start)
                .Take(length)
                .ToListAsync();
        }
    }
}
