using Microsoft.AspNetCore.Identity;
using System;

namespace TodoListApi.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string TimezoneInfoId { get; set; }
    }
}
