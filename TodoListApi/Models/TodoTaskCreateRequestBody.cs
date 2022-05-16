using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApi.Models
{
    public class TodoTaskCreateRequestBody
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Title { get; set; }
        public string Deadline { get; set; } = DateTimeOffset.Now.AddHours(24).ToString();
        public bool Done { get; set; } = false;
    }
}
