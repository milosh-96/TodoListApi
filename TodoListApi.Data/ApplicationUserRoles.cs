using System;
using System.Collections.Generic;
using System.Text;

namespace TodoListApi.Data
{
    public static class ApplicationUserRoles
    {
        public static string Administrator { get; set; } = "Administrator";
        public static string User { get; set; } = "User";
        public static string Moderator { get; set; } = "Moderator";
    }
}
