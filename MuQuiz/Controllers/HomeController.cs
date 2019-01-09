using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuQuiz.Models;

namespace MuQuiz.Controllers
{
    public class HomeController : Controller
    {
        private readonly SessionStorageService sessionService;

        public HomeController(SessionStorageService sessionStorageService)
        {
            sessionService = sessionStorageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string GameId)
        {
            if (!ModelState.IsValid)
                return View();

            sessionService.GameId = GameId;
            return RedirectToAction(nameof(PlayerController.Index), "Player");
        }
    }
}