using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : Controller
    {
        public string Login()
        {
            return "your token";
        }
    }
}
