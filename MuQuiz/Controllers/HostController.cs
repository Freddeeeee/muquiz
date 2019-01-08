using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MuQuiz.Models.ViewModels;

namespace MuQuiz.Controllers
{
    public class HostController : Controller
    {
        public IActionResult Index()
        {
            // to-do: add game id generator (constructor or helper class?)
            var vm = new HostIndexVM { GameId = "TEST" };

            return View(vm);
        }
    }
}