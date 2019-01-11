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
        private readonly GameService gameService;

        public HomeController(GameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string GameId)
        {
            if (!await gameService.SessionIsActive(GameId))
            {
                ModelState.AddModelError("GameId", "We can't find this ID... :(");
            }

            if (!ModelState.IsValid)
                return View();

            return RedirectToAction(nameof(PlayerController.Index), "Player", new { gameId });
        }
    }
}