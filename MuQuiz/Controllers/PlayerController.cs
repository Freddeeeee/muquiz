using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuQuiz.Models.ViewModels;

namespace MuQuiz.Controllers
{
    public class PlayerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PlayerIndexVM vm)
        {
            //to-do: validate whether key is still valid
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            HttpContext.Session.SetString("Name", vm.Name);

            return RedirectToAction(nameof(Game));
        }

        public IActionResult Game()
        {
            var vm = new PlayerGameVM
            {
                GameId = HttpContext.Session.GetString("GameId"),
                Name = HttpContext.Session.GetString("Name"),
            };

            return View(vm);
        }

        public IActionResult ShowAlternatives(string song)
        {
            // to-do: retrieve alternatives from data storage
            return PartialView("~/Views/Shared/Player/_Alternatives.cshtml");
        }

        public IActionResult ShowWaitingScreen()
        {
            return PartialView("~/Views/Shared/Player/_WaitingScreen.cshtml");
        }

        public IActionResult ShowFinalPosition(int position)
        {
            //to-do: instead receive userId and find position in DB?
            return PartialView("~/Views/Shared/Player/_FinalPosition.cshtml", position);
        }
    }
}