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
    }
}