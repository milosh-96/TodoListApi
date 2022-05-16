using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Data;

namespace TodoListApi.Models
{
    public class TodoListObjectResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime TodoListDate { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public List<TodoTask> Tasks { get; set; }
    }
}
