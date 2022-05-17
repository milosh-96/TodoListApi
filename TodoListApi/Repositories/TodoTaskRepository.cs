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
            throw new NotImplementedException();
        }

        public TodoTask EditTodoTask(string id, TodoList todoList, ApplicationUser user, TodoTaskEditRequestBody requestBody)
        {
            throw new NotImplementedException();
        }

        public TodoTask GetTodoTask(string id)
        {
            throw new NotImplementedException();
        }

        public TodoTask GetTodoTask(string id, TodoList todoList)
        {
            throw new NotImplementedException();
        }

        public TodoTask GetTodoTask(string id, TodoList todoList, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public List<TodoTask> GetTodoTasks(TodoList todoList)
        {
            throw new NotImplementedException();
        }

        public List<TodoTask> GetTodoTasks(TodoList todoList, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskStatus(string id, TodoList todoList, ApplicationUser user, bool taskStatus)
        {
            throw new NotImplementedException();
        }
    }
}
