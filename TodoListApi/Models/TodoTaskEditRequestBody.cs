using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApi.Models
{
    public class TodoTaskEditRequestBody
    {
        [StringLength(255, MinimumLength = 2)]
        public string Title { get; set; }
        public string Deadline { get; set; }
        public bool? Done { get; set; }
    }
}
