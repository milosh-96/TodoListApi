using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Data;

namespace TodoListApi.Repositories
{
    public interface ICompletedTaskRepository
    {
        public CompletedTaskUser GetCompletedTask(string id);
        public CompletedTaskUser GetCompletedTaskByTodoTask(TodoTask task);
        public List<CompletedTaskUser> GetCompletedTasksOfTodoList(TodoList todoList);
        public List<CompletedTaskUser> GetCompletedTasksOfTodoList(TodoList todoList, ApplicationUser user);
        public List<CompletedTaskUser> GetCompletedTasksOfUser(ApplicationUser user);
        public CompletedTaskUser AddCompletedTask(TodoTask todoTask);
        public void DeleteCompletedTask(string id);
        public void DeleteCompletedTaskByTodoTask(TodoTask todoTask);

        public void Save();
    }
}
