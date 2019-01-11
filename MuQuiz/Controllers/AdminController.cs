using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MuQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [Route("/admin/index")]
        public IActionResult Index()
        {
            return Content("ADMINSIDA!!!!");
        }
    }
}