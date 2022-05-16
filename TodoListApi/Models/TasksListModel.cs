using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Data;

namespace TodoListApi.Models
{
    public class TasksListModel
    {
        public string TodoListId { get; set; }
        public List<TodoTask> Tasks { get; set; }

        public int TotalPages { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
    }
}
