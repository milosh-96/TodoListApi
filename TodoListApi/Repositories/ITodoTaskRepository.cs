using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Data;
using TodoListApi.Models;

namespace TodoListApi.Repositories
{
    public interface ITodoTaskRepository
    {
        public void Save();

        public TodoTask GetTodoTask(string id);
        public TodoTask GetTodoTask(string id,TodoList todoList);
        public TodoTask GetTodoTask(string id, TodoList todoList,ApplicationUser user);
        public List<TodoTask> GetTodoTasks(TodoList todoList);
        public List<TodoTask> GetTodoTasks(TodoList todoList,ApplicationUser user);

        public TodoTask AddTodoTask(TodoList todoList,ApplicationUser user,TodoTaskCreateRequestBody requestBody);
        public TodoTask EditTodoTask(string id, TodoList todoList, ApplicationUser user, TodoTaskEditRequestBody requestBody);

        public void DeleteTodoTask(string id, TodoList todoList, ApplicationUser user);

        public void UpdateTaskStatus(string id, TodoList todoList, ApplicationUser user, bool taskStatus);
    }
}
