using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace TodoListApi.Data
{
    public class TodoTask
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();


        public string TodoListId { get; set; }

        [JsonIgnore]
        public TodoList TodoList { get; set; }


        public string Title { get; set; }

        public DateTimeOffset Deadline { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public bool Done { get; set; }


    }
}
