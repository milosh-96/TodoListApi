using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Data;
using TodoListApi.Models;

namespace TodoListApi.Repositories
{
    public interface ITodoListRepository
    {
        public void Save();
        public TodoList GetTodoList(string id);

        // get todo list and check if the user owns it //
        public TodoList GetTodoList(string id, ApplicationUser user);
        public TodoList GetTodoListWithTasks(string id, ApplicationUser user);

        public List<TodoList> GetTodoLists();

        // get all todo lists of provided user //
        public List<TodoList> GetTodoLists(ApplicationUser user);


        public TodoList AddTodoList(TodoListCreateRequestBody requestBody,ApplicationUser user);

        public TodoList UpdateTodoList(string id,TodoListEditRequestBody requestBody, ApplicationUser user);

        public void DeleteTodoList(string id, ApplicationUser user);

    }
}
