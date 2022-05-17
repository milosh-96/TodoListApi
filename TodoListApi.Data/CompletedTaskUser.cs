using System;
using System.Collections.Generic;
using System.Text;

namespace TodoListApi.Data
{
    public class CompletedTaskUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public TodoTask Task { get; set; }
        public string   TaskId { get; set; }

        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    }
}
