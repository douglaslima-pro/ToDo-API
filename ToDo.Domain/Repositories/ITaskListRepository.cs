using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Entities.Tasks;
using ToDo.Domain.Repositories.Base;

namespace ToDo.Domain.Repositories
{
    public interface ITaskListRepository : ICrudRepository<TaskList, int>
    {
        Task<IEnumerable<TaskListItem>> GetAllTasksFromListAsync(
            int taskListId,
            Expression<Func<TaskListItem, object?>>? orderByExpression = null,
            bool isAscending = true,
            int start = 0,
            int length = 10);
    }
}
