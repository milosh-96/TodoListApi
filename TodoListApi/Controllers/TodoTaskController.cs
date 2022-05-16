﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TodoListApi.Data;
using TodoListApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoListApi.Controllers
{
    [Route("api/TodoList/{todoListId}/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(ApplicationUserRoles.User))]
    public class TodoTaskController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoTaskController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // GET: api/<TodoTaskController>
        [HttpGet]
        public TasksListModel Get(string todoListId,[FromQuery] int page = 1)
        {
            IQueryable<TodoList> initialQuery = _dbContext.TodoLists.Where(x => x.UserId == _userManager.GetUserId(User))
                .Where(x => x.Id == todoListId);
            if (initialQuery.Any()) {
                TodoList todoList = new TodoList();
                todoList = initialQuery.Include(x => x.Tasks).FirstOrDefault();
                TasksListModel listModel = new TasksListModel();
                listModel.TodoListId = todoList.Id;
                listModel.CurrentPage = page;
                if(listModel.CurrentPage == 1) {
                    listModel.Tasks = todoList.Tasks.Skip(0).Take(listModel.ItemsPerPage).ToList();
                }
                else
                {
                    listModel.Tasks = todoList.Tasks.Skip((listModel.CurrentPage-1) * listModel.ItemsPerPage).Take(listModel.ItemsPerPage).ToList();
                }
                listModel.TotalPages = (int)Math.Ceiling(decimal.Divide(todoList.Tasks.Count,listModel.ItemsPerPage));
                return listModel;
            }
            return new TasksListModel();
        }

        // GET api/<TodoTaskController>/5
        [HttpGet("{id}")]
        public EntityHttpResponse<TodoTask> Get(string id,string todoListId)
        {
            IQueryable<TodoTask> initialQuery = _dbContext.TodoTasks.Where(x => x.TodoListId == todoListId).Where(x => x.Id == id);
            if(initialQuery.Any()) { 
                EntityHttpResponse<TodoTask> response = new EntityHttpResponse<TodoTask>();
                TodoTask todoTask = initialQuery.FirstOrDefault();
                response.StatusCode = HttpStatusCode.OK;
                response.Item = todoTask;
                return response;
            }
            return new EntityHttpResponse<TodoTask>() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found" };
        
        }

        // POST api/<TodoTaskController>
        [HttpPost]
        public EntityHttpResponse<TodoTask> Post(string todoListId, [FromBody] TodoTaskCreateRequestBody request)
        {
            IQueryable<TodoList> initialTodoListQuery = _dbContext.TodoLists
                .Where(u => u.UserId == _userManager.GetUserId(User))
                .Where(t => t.Id == todoListId);
            // check if todo list exists and does that list belongs to current user //
            if (initialTodoListQuery.Any())
            {
                EntityHttpResponse<TodoTask> response = new EntityHttpResponse<TodoTask>();
                TodoList todoList = initialTodoListQuery.Include(x => x.Tasks).FirstOrDefault();
                TodoTask todoTask = new TodoTask();
                todoTask.Title = request.Title;
                todoTask.Done = request.Done;
                try
                {
                    todoTask.Deadline = (request.Deadline != null) ? DateTimeOffset.Parse(request.Deadline) : todoTask.Deadline;
                }
                catch (Exception e)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = e.Message;
                    return response;
                }

                todoList.Tasks.Add(todoTask);
                if (_dbContext.SaveChanges() > 0)
                {
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = "Todo task is created.";
                    response.Item = todoTask;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Error happened.";
                }
                return response;

            }
            return new EntityHttpResponse<TodoTask>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "TodoList with specified ID wasn't found or it doesn't belong to the current user.",
            };
        }

        // PUT api/<TodoTaskController>/5
        [HttpPut("{id}")]
        public EntityHttpResponse<TodoTask> Put(string id, string todoListId, [FromBody] TodoTaskEditRequestBody request)
        {
            IQueryable<TodoList> initialTodoListQuery = _dbContext.TodoLists
                .Where(u => u.UserId == _userManager.GetUserId(User))
                .Where(t => t.Id == todoListId);
            // check if todo list exists and does that list belongs to current user //
            if (initialTodoListQuery.Any())
            {
                EntityHttpResponse<TodoTask> response = new EntityHttpResponse<TodoTask>();

                TodoList todoList = initialTodoListQuery.Include(x => x.Tasks).FirstOrDefault();
                TodoTask todoTask = todoList.Tasks.Where(x => x.Id == id).FirstOrDefault();
                todoTask.Title = request.Title ?? todoTask.Title;
                todoTask.Done = (request.Done != null) ? request.Done.Value : todoTask.Done;
                try
                {
                    todoTask.Deadline = (request.Deadline != null) ? DateTimeOffset.Parse(request.Deadline) : todoTask.Deadline;
                }
                catch (Exception e)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = e.Message;
                    return response;
                }
                todoTask.ModifiedAt = DateTime.UtcNow;


                todoList.Tasks.Add(todoTask);
                if (_dbContext.SaveChanges() > 0)
                {
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = "Todo task is modified.";
                    response.Item = todoTask;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Error happened.";
                }
                return response;

            }
            return new EntityHttpResponse<TodoTask>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "TodoList with specified ID wasn't found or it doesn't belong to the current user.",
            };
        }

        // DELETE api/<TodoTaskController>/5
        [HttpDelete("{id}")]
        public EntityHttpResponse<TodoTask> Delete(string id, string todoListId)
        {
            IQueryable<TodoList> initialTodoListQuery = _dbContext.TodoLists
              .Where(u => u.UserId == _userManager.GetUserId(User))
              .Where(t => t.Id == todoListId);
            // check if todo list exists and does that list belongs to current user //
            if (initialTodoListQuery.Any())
            {
                EntityHttpResponse<TodoTask> response = new EntityHttpResponse<TodoTask>();

                TodoList todoList = initialTodoListQuery.Include(x => x.Tasks).FirstOrDefault();
                TodoTask todoTask = todoList.Tasks.Where(x => x.Id == id).FirstOrDefault();
                _dbContext.TodoTasks.Remove(todoTask);
                if (_dbContext.SaveChanges() > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "TodoTask is deleted.";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "There was an error.";
                }
                return response;
            }
            return new EntityHttpResponse<TodoTask>() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found." };
        }
    }
}
