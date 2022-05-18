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
using TodoListApi.Repositories;

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

        private readonly ITodoListRepository _todoListRepository;
        private readonly ICompletedTaskRepository _completedTaskRepository;

        public TodoListController(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager, ITodoListRepository todoListRepository, ICompletedTaskRepository completedTaskRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _todoListRepository = todoListRepository;
            _completedTaskRepository = completedTaskRepository;
        }

        // GET: api/<TodoListController>
        [HttpGet]
        public async Task<List<TodoList>> Get()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            return _todoListRepository.GetTodoLists(user);
        }

        // GET api/<TodoListController>/5
        [HttpGet("{id}")]
        public async Task<EntityHttpResponse<TodoList>> Get(string id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            TodoList todoList = _todoListRepository.GetTodoList(id, user);
            if (todoList != null) {
                return new EntityHttpResponse<TodoList>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Item returned.",
                    Item = todoList
                };
            }
            return new EntityHttpResponse<TodoList>() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found." };
        }

        // POST api/<TodoListController>
        [HttpPost]
        public async Task<EntityHttpResponse<TodoList>> Post([FromBody] TodoListCreateRequestBody request)
        {
            EntityHttpResponse<TodoList> response = new EntityHttpResponse<TodoList>();
            
            try
            {
                TodoList todoList = _todoListRepository.AddTodoList(request, await _userManager.GetUserAsync(User));
                _todoListRepository.Save();
                response.StatusCode = HttpStatusCode.Created;
                response.Message = "TodoList is created.";
                response.Item = todoList;
            }
            catch(Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = e.Message;
            }
            return response;

        }

        // PUT api/<TodoListController>/5
        [HttpPut("{id}")]
        public async Task<EntityHttpResponse<TodoList>> Put(string id, [FromBody] TodoListEditRequestBody request)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            TodoList todoList = _todoListRepository.GetTodoList(id, user);
            if(todoList != null)
            {
                EntityHttpResponse<TodoList> response = new EntityHttpResponse<TodoList>();
                try {
                    
                    todoList = _todoListRepository.UpdateTodoList(id, request, user);
                    _todoListRepository.Save();
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "TodoList is modified.";
                    response.Item = todoList;
                }
                catch(Exception e)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = e.Message;
                }
                return response;
            }
            return new EntityHttpResponse<TodoList>() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found" };
        }

        // DELETE api/<TodoListController>/5
        [HttpDelete("{id}")]
        public async Task<EntityHttpResponse<TodoList>> Delete(string id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            TodoList todoList = _todoListRepository.GetTodoList(id, user);
            if (todoList != null)
            {
                EntityHttpResponse<TodoList> response = new EntityHttpResponse<TodoList>();
                try 
                {
                    _todoListRepository.DeleteTodoList(id, user);
                    _todoListRepository.Save();
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "TodoList is deleted.";
                }
                catch(Exception e)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = e.Message;
                }
                return response;
            }
            return new EntityHttpResponse<TodoList>() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found" };
        }
    }
}
