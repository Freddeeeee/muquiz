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
        private readonly GameService gameService;

        public HomeController(SessionStorageService sessionStorageService, GameService gameService)
        {
            sessionService = sessionStorageService;
            this.gameService = gameService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string GameId)
        {
            if (!gameService.SessionIsActive(GameId))
            {
                ModelState.AddModelError("GameId", "We can't find this ID... :(");
            }

            if (!ModelState.IsValid)
                return View();

            sessionService.GameId = GameId;
            return RedirectToAction(nameof(PlayerController.Index), "Player");
        }
    }
}