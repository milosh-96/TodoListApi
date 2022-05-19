using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Data;

namespace TodoListApi.Models
{
    public class TodoListsListModel
    {
        public List<TodoList> Lists { get; set; }

        [StringLength(200, MinimumLength = 2)]

        public string FilterBy { get; set; }

        [StringLength(200,MinimumLength = 2)]
        public string FilterQuery { get; set; }
        public int TotalPages { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;

        public List<string> Errors { get; set; } = new List<string>();


    }
    public static class TodoListsListModelFilters
    {
        public static string TodoListDate { get; set; } = "TodoListDate";
        public static string Title { get; set; } = "Title";
    }
}
