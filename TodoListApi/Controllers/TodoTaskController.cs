using Microsoft.AspNetCore.Authorization;
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
using TodoListApi.Repositories;

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
        private readonly ITodoTaskRepository _todoTaskRepository;
        private readonly ITodoListRepository _todoListRepository;
        private readonly ICompletedTaskRepository _completedTaskRepository;

        public TodoTaskController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ITodoTaskRepository todoTaskRepository, ITodoListRepository todoListRepository, ICompletedTaskRepository completedTaskRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _todoTaskRepository = todoTaskRepository;
            _todoListRepository = todoListRepository;
            _completedTaskRepository = completedTaskRepository;
        }

        // GET: api/<TodoTaskController>
        [HttpGet]
        public async Task<TasksListModel> Get(string todoListId, [FromQuery] TasksListModel request)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            TodoList todoList = _todoListRepository.GetTodoListWithTasks(todoListId, user);
            if (todoList != null)
            {
                List<TodoTask> tasks = todoList.Tasks;
                TasksListModel listModel = request ?? new TasksListModel();
                listModel.TodoListId = todoList.Id;

                switch (listModel.FilterBy)
                {
                    case nameof(TasksListModelFilters.Deadline):
                        try
                        {
                            DateTimeOffset filterDate = DateTimeOffset.Parse(listModel.FilterQuery);
                            tasks = todoList.Tasks.Where(x => x.Deadline.Date == filterDate.Date).ToList();
                        }
                        catch (Exception e)
                        {
                            listModel.Errors.Add(e.Message);
                        }
                        break;
                    case nameof(TasksListModelFilters.Done):
                        try
                        {
                            bool isDone = bool.Parse(listModel.FilterQuery);
                            tasks = todoList.Tasks.Where(x => x.Done == isDone).ToList();
                        }
                        catch (Exception e)
                        {
                            listModel.Errors.Add(e.Message);
                        }
                        break;
                }

                if (listModel.CurrentPage == 1)
                {
                    listModel.Tasks = tasks.Skip(0).Take(listModel.ItemsPerPage).ToList();
                }
                else
                {
                    listModel.Tasks = tasks.Skip((listModel.CurrentPage - 1) * listModel.ItemsPerPage).Take(listModel.ItemsPerPage).ToList();
                }
                listModel.TotalPages = (int)Math.Ceiling(decimal.Divide(tasks.Count, listModel.ItemsPerPage));
                return listModel;
            }
            return new TasksListModel();
        }

        // GET api/<TodoTaskController>/5
        [HttpGet("{id}")]
        public async Task<EntityHttpResponse<TodoTask>> Get(string id, string todoListId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            TodoTask todoTask = _todoTaskRepository.GetTodoTask(id, _todoListRepository.GetTodoList(todoListId), user);
            if (todoTask != null)
            {
                EntityHttpResponse<TodoTask> response = new EntityHttpResponse<TodoTask>();
                response.StatusCode = HttpStatusCode.OK;
                response.Item = todoTask;
                return response;
            }
            return new EntityHttpResponse<TodoTask>() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found" };

        }

        // POST api/<TodoTaskController>
        [HttpPost]
        public async Task<EntityHttpResponse<TodoTask>> Post(string todoListId, [FromBody] TodoTaskCreateRequestBody request)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            // check if todo list exists and does that list belongs to current user //
            TodoList todoList = _todoListRepository.GetTodoListWithTasks(todoListId, user);
            if (todoList != null)
            {
                EntityHttpResponse<TodoTask> response = new EntityHttpResponse<TodoTask>();
                try
                {
                    TodoTask todoTask = _todoTaskRepository.AddTodoTask(todoList, user, request);
                    _todoTaskRepository.Save();
                    if (request.Done == true)
                    {
                        _completedTaskRepository.AddCompletedTask(todoTask);
                    }
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = "Todo task is created.";
                    response.Item = todoTask;
                }
                catch (Exception e)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = e.Message;
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
        public async Task<EntityHttpResponse<TodoTask>> Put(string id, string todoListId, [FromBody] TodoTaskEditRequestBody request)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            TodoList todoList = _todoListRepository.GetTodoList(todoListId, user);
            // check if todo list exists and does that list belongs to current user //
            if (todoList != null)
            {
                EntityHttpResponse<TodoTask> response = new EntityHttpResponse<TodoTask>();
                try
                {
                    TodoTask todoTask = _todoTaskRepository.EditTodoTask(id, todoList, user, request);
                    _todoTaskRepository.Save();
                    if (request.Done == true)
                    {
                        _completedTaskRepository.AddCompletedTask(todoTask);
                        _completedTaskRepository.Save();
                    }
                    else
                    {
                        CompletedTaskUser completedTask = _completedTaskRepository.GetCompletedTaskByTodoTask(todoTask);
                        if (completedTask != null)
                        {
                            _completedTaskRepository.DeleteCompletedTask(completedTask.Id);
                            _completedTaskRepository.Save();
                        }
                    }
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = "Todo task is modified.";
                    response.Item = todoTask;
                }
                catch (Exception e)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = e.Message;
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
        public async Task<EntityHttpResponse<TodoTask>> Delete(string id, string todoListId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            TodoList todoList = _todoListRepository.GetTodoList(todoListId, user);
            // check if todo list exists and does that list belongs to current user //
            if (todoList != null)
            {
                EntityHttpResponse<TodoTask> response = new EntityHttpResponse<TodoTask>();

                try
                {
                    _todoTaskRepository.DeleteTodoTask(id, todoList, user);
                    _todoTaskRepository.Save();
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "TodoTask is deleted.";
                }
                catch (Exception e)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = e.Message;
                }
                return response;
            }
            return new EntityHttpResponse<TodoTask>() { StatusCode = HttpStatusCode.NotFound, Message = "Item not found." };
        }
    }
}
