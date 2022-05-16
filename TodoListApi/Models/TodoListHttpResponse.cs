using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TodoListApi.Data;

namespace TodoListApi.Models
{
    public class TodoListHttpResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public TodoList TodoList { get; set; }
    }
}
