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

        public IActionResult HostGame()
        {
            return View();
        }

        public IActionResult ShowAlternatives(string song)
        {
            // to-do: retrieve alternatives from data storage
            return PartialView("~/Views/Shared/Host/_ShowAlternatives.cshtml");
        }

        public IActionResult ShowResults()
        {
            return PartialView("~/Views/Shared/Host/_ShowResults.cshtml");
        }

        public IActionResult ShowFinalResults()
        {
            return PartialView("~/Views/Shared/Host/_ShowFinalResults.cshtml");
        }
    }
}