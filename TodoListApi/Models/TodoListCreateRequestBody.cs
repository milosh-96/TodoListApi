using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApi.Models
{
    public class TodoListCreateRequestBody
    {
     
        [Required]
        [StringLength(255,MinimumLength = 2)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime TodoListDate { get; set; } = DateTime.UtcNow;

    }
}
