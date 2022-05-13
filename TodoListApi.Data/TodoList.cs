using System;
using System.Collections.Generic;
using System.Text;

namespace TodoListApi.Data
{
    public class TodoList
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime TodoListDate { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public List<TodoTask> Tasks { get; set; }
    }
}
