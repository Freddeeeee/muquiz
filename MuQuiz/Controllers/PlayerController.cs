using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuQuiz.Models;
using MuQuiz.Models.ViewModels;

namespace MuQuiz.Controllers
{
    public class PlayerController : Controller
    {
        private readonly SessionStorageService sessionService;
        private readonly QuestionService questionService;
        private readonly GameService gameService;

        public PlayerController(SessionStorageService sessionStorageService, QuestionService questionService, GameService gameService)
        {
            sessionService = sessionStorageService;
            this.questionService = questionService;
            this.gameService = gameService;
        }

        [HttpGet]
        public IActionResult Index(string gameId)
        {
            sessionService.GameId = gameId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PlayerIndexVM vm)
        {
            if (!await gameService.SessionIsActive(sessionService.GameId))
            {
                ModelState.AddModelError("AccessError", "The session you joined has timed out.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            sessionService.Name = vm.Name;

            return RedirectToAction(nameof(Game));
        }

        public IActionResult Game()
        {
            var vm = new PlayerGameVM
            {
                GameId = sessionService.GameId,
                Name = sessionService.Name,
                AvatarCode = gameService.GetRandomAvatar()
            };

            return View(vm);
        }

        public IActionResult ShowAlternatives(int song)
        {
            var alternatives = questionService.GetQuestionsForId(song);
            return PartialView("~/Views/Shared/Player/_Alternatives.cshtml", alternatives);
        }

        public IActionResult ShowWaitingScreen()
        {
            return PartialView("~/Views/Shared/Player/_WaitingScreen.cshtml");
        }

        public IActionResult ShowFinalPosition(int position)
        {
            return PartialView("~/Views/Shared/Player/_FinalPosition.cshtml", position);
        }

        public IActionResult ShowSessionClosedScreen()
        {
            return PartialView("~/Views/Shared/Player/_SessionClosedScreen.cshtml");
        }
    }
}