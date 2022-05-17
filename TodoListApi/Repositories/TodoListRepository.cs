using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Data;
using TodoListApi.Models;

namespace TodoListApi.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoListRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void DeleteTodoList(string id,ApplicationUser user)
        {
            TodoList todoList = this.GetTodoList(id, user);
            _dbContext.TodoLists.Remove(todoList);
        }

        public TodoList UpdateTodoList(string id,TodoListEditRequestBody requestBody, ApplicationUser user)
        {
            TodoList todoList = this.GetTodoList(id,user);

            todoList.ModifiedAt = DateTime.UtcNow;
            todoList.Title = (todoList.Title == requestBody.Title) ? todoList.Title : requestBody.Title;
            todoList.Description = (todoList.Description == requestBody.Description) ? todoList.Description : requestBody.Description;
            try
            {
                todoList.TodoListDate = DateTime.Parse(requestBody.TodoListDate);
            }
            catch(Exception e)
            {
                
            }
            return todoList;
        }

        public TodoList AddTodoList(TodoListCreateRequestBody requestBody,ApplicationUser user)
        {
            TodoList todoList = new TodoList()
            {
                Title = requestBody.Title,
                Description = requestBody.Description,
                TodoListDate = DateTime.Parse(requestBody.TodoListDate),
                UserId = user.Id
            };
            _dbContext.TodoLists.Add(todoList);
            return todoList;
        }

        public TodoList GetTodoList(string id)
        {
            return _dbContext.TodoLists.Where(x => x.Id == id).FirstOrDefault();
        }

        public TodoList GetTodoList(string id, ApplicationUser user)
        {
            return _dbContext.TodoLists.Where(x => x.Id == id)
                .Where(x=>x.UserId==user.Id)
                .FirstOrDefault();
        }

        public List<TodoList> GetTodoLists()
        {
            return _dbContext.TodoLists.ToList();
        }

        public List<TodoList> GetTodoLists(ApplicationUser user)
        {
            return _dbContext.TodoLists.Where(x => x.UserId == user.Id).ToList();
        }

        public TodoList GetTodoListWithTasks(string id, ApplicationUser user)
        {
            return _dbContext.TodoLists.Include(t=>t.Tasks).Where(x => x.Id == id)
               .Where(x => x.UserId == user.Id)
               .FirstOrDefault();
        }
    }
}
