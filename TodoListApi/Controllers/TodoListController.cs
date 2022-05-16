using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(ApplicationUserRoles.User))]

    public class TodoListController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoListController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // GET: api/<TodoListController>
        [HttpGet]
        public IEnumerable<TodoList> Get()
        {
            string userId = _userManager.GetUserId(User);
            IQueryable<TodoList> initialQuery = _dbContext.TodoLists.Where(x => x.UserId == userId);
            return initialQuery.ToList();
        }

        // GET api/<TodoListController>/5
        [HttpGet("{id}")]
        public TodoListHttpResponse Get(string id)
        {
            IQueryable<TodoList> initialQuery = _dbContext.TodoLists
                .Where(x => x.UserId == _userManager.GetUserId(User))
                .Where(x => x.Id == id);

            if(initialQuery.Any()) {
                return new TodoListHttpResponse()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Item returned.",
                    TodoList = initialQuery.FirstOrDefault()
                };
            }
            return new TodoListHttpResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found." };
        }

        // POST api/<TodoListController>
        [HttpPost]
        public TodoListHttpResponse Post([FromBody] TodoListCreateRequestBody request)
        {
            TodoList todoList = new TodoList() {
                Title = request.Title,
                Description = request.Description,
                TodoListDate = request.TodoListDate,
                UserId=_userManager.GetUserId(User)
            };
            TodoListHttpResponse response = new TodoListHttpResponse();
            _dbContext.TodoLists.Add(todoList);
            if (_dbContext.SaveChanges() > 0)
            {
                response.StatusCode = HttpStatusCode.Created;
                response.Message = "TodoList is created.";
                response.TodoList = todoList;
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "There was an error.";
            }
            return response;

        }

        // PUT api/<TodoListController>/5
        [HttpPut("{id}")]
        public TodoListHttpResponse Put(string id, [FromBody] TodoListEditRequestBody request)
        {
            IQueryable<TodoList> initialQuery = _dbContext.TodoLists
                .Where(x => x.UserId == _userManager.GetUserId(User))
                .Where(x => x.Id == id);
            if(initialQuery.Any())
            {
                TodoList todoList = initialQuery.FirstOrDefault();
                todoList.Title = request.Title ?? todoList.Title;
                todoList.Description = request.Description ?? todoList.Title;
                todoList.TodoListDate = (request.TodoListDate != null) ? DateTime.Parse(request.TodoListDate) : todoList.TodoListDate;
                todoList.ModifiedAt = DateTime.UtcNow;

                TodoListHttpResponse response = new TodoListHttpResponse();

                 if (_dbContext.SaveChanges() > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "TodoList is modified.";
                    response.TodoList = todoList;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "There was an error.";
                }
                return response;
            }
            return new TodoListHttpResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found" };
        }

        // DELETE api/<TodoListController>/5
        [HttpDelete("{id}")]
        public TodoListHttpResponse Delete(string id)
        {
            IQueryable<TodoList> initialQuery = _dbContext.TodoLists
               .Where(x => x.UserId == _userManager.GetUserId(User))
               .Where(x => x.Id == id);
            if (initialQuery.Any())
            {
                TodoList todoList = initialQuery.FirstOrDefault();

                _dbContext.TodoLists.Remove(todoList);

                TodoListHttpResponse response = new TodoListHttpResponse();

                if (_dbContext.SaveChanges() > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "TodoList is deleted.";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "There was an error.";
                }
                return response;
            }
            return new TodoListHttpResponse() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found" };
        }
    }
}
