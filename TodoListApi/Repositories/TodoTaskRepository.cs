using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Data;
using TodoListApi.Models;

namespace TodoListApi.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoTaskRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TodoTask AddTodoTask(TodoList todoList, ApplicationUser user, TodoTaskCreateRequestBody requestBody)
        {
            if (todoList != null)
            {
                TodoTask todoTask = new TodoTask();
                todoTask.Title = requestBody.Title;
                todoTask.Done = requestBody.Done;
                todoTask.TodoListId = todoList.Id;
                try
                {
                    todoTask.Deadline = DateTimeOffset.Parse(requestBody.Deadline);
                }
                catch (Exception e)
                {
                    todoTask.Deadline = DateTimeOffset.UtcNow;
                }
                todoTask.Deadline = TimeZoneInfo.ConvertTime(todoTask.Deadline, TimeZoneInfo.FindSystemTimeZoneById(user.TimezoneInfoId));                _dbContext.TodoTasks.Add(todoTask);
                return todoTask;
            }
            return null;
        }

        public void DeleteTodoTask(string id, TodoList todoList, ApplicationUser user)
        {
            TodoTask todoTask = this.GetTodoTask(id, todoList, user);
            if(todoTask != null)
            {
                _dbContext.TodoTasks.Remove(todoTask);
            }
        }

        public TodoTask EditTodoTask(string id, TodoList todoList, ApplicationUser user, TodoTaskEditRequestBody requestBody)
        {
            TodoTask todoTask = this.GetTodoTask(id, todoList, user);
            if(todoTask != null)
            {
                todoTask.Title = requestBody.Title ?? todoTask.Title;
                todoTask.Done = (requestBody.Done != null) ? requestBody.Done.Value : todoTask.Done;
                try
                {
                    todoTask.Deadline = DateTimeOffset.Parse(requestBody.Deadline);
                }
                catch(Exception e)
                {
                    todoTask.Deadline = todoTask.Deadline;
                }
                todoTask.Deadline = TimeZoneInfo.ConvertTime(todoTask.Deadline, TimeZoneInfo.FindSystemTimeZoneById(user.TimezoneInfoId));
                todoTask.ModifiedAt = DateTime.UtcNow;
                
                return todoTask;
            }
            return null;
        }

        public TodoTask GetTodoTask(string id)
        {
            return _dbContext.TodoTasks.Where(x => x.Id == id).FirstOrDefault();
        }

        public TodoTask GetTodoTask(string id, TodoList todoList)
        {
            return _dbContext.TodoTasks.Where(x => x.Id == id).Where(t => t.TodoListId == todoList.Id).FirstOrDefault();
        }

        // get todolist task and ensure it belongs to the user's todolist
        public TodoTask GetTodoTask(string id, TodoList todoList, ApplicationUser user)
        {
            if (todoList.UserId == user.Id)
            {
                return _dbContext.TodoTasks.Include(x => x.TodoList).Where(x => x.TodoListId == todoList.Id).Where(x => x.Id == id).FirstOrDefault();
            }
            return null;
        }

        public List<TodoTask> GetTodoTasks(TodoList todoList)
        {
            return _dbContext.TodoTasks.Where(x => x.TodoListId == todoList.Id).ToList();
        }

        public List<TodoTask> GetTodoTasks(TodoList todoList, ApplicationUser user)
        {
            return _dbContext.TodoTasks
                .Include(x=>x.TodoList)
                .Where(x => x.TodoListId == todoList.Id)
                .Where(t=>t.TodoList.UserId==user.Id)
                .ToList();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateTaskStatus(string id, TodoList todoList, ApplicationUser user, bool taskStatus)
        {
            throw new NotImplementedException();
        }
    }
}
