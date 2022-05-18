using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Data;

namespace TodoListApi.Repositories
{
    public class CompletedTaskRepository : ICompletedTaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompletedTaskRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CompletedTaskUser AddCompletedTask(TodoTask todoTask)
        {
                CompletedTaskUser completedTaskUser = new CompletedTaskUser()
                {
                    CompletedAt = DateTime.UtcNow,
                    TaskId = todoTask.Id,
                };
                _dbContext.CompletedTasksUsers.Add(completedTaskUser);
                return completedTaskUser;
        }

        public void DeleteCompletedTask(string id)
        {
            CompletedTaskUser completedTaskUser = this.GetCompletedTask(id);
            if (completedTaskUser != null)
            {
                _dbContext.CompletedTasksUsers.Remove(completedTaskUser);
            }
        }

        public void DeleteCompletedTaskByTodoTask(TodoTask todoTask)
        {
            CompletedTaskUser completedTaskUser = this.GetCompletedTaskByTodoTask(todoTask);
            if (completedTaskUser != null)
            {
                _dbContext.CompletedTasksUsers.Remove(completedTaskUser);
            }
        }

        public CompletedTaskUser GetCompletedTask(string id)
        {
            return _dbContext.CompletedTasksUsers.Where(x => x.Id == id).FirstOrDefault();
        }

        public CompletedTaskUser GetCompletedTaskByTodoTask(TodoTask task)
        {
            return _dbContext.CompletedTasksUsers.Where(x => x.TaskId == task.Id).FirstOrDefault();
        }

        public List<CompletedTaskUser> GetCompletedTasksOfTodoList(TodoList todoList)
        {
            return _dbContext.CompletedTasksUsers.Include(x => x.Task).Where(x => x.Task.TodoListId == todoList.Id).ToList();
        }

        public List<CompletedTaskUser> GetCompletedTasksOfTodoList(TodoList todoList, ApplicationUser user)
        {
            return _dbContext.CompletedTasksUsers.Include(x => x.Task).ThenInclude(x=>x.TodoList)
                .Where(x => x.Task.TodoListId == todoList.Id)
                .Where(x=>x.Task.TodoList.UserId==user.Id)
                .ToList();
        }

        public List<CompletedTaskUser> GetCompletedTasksOfUser(ApplicationUser user)
        {
            return _dbContext.CompletedTasksUsers.Include(x => x.Task).ThenInclude(x => x.TodoList)
                .Where(x => x.Task.TodoList.UserId == user.Id)
                .ToList();
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
