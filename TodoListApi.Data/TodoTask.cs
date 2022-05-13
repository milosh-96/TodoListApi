using System;
using System.Collections.Generic;
using System.Text;

namespace TodoListApi.Data
{
    public class TodoTask
    {
        public string Id { get; set; }

        public string TodoListId { get; set; }
        public TodoList TodoList { get; set; }


        public string Title { get; set; }

        public DateTimeOffset Deadline { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public bool Done { get; set; }


    }
}
