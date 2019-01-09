using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuQuiz.Models;
using MuQuiz.Models.ViewModels;
using Newtonsoft.Json;

namespace MuQuiz.Controllers
{
    [Authorize]
    public class HostController : Controller
    {
        HostService service;
        SpotifyService spotify;
        private readonly QuestionService questionService;
        SessionStorageService sessionService;

        public HostController(HostService service, SessionStorageService sessionStorageService, SpotifyService spotify, QuestionService questionService)
        {
            this.service = service;
            sessionService = sessionStorageService;
            this.spotify = spotify;
            this.questionService = questionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = new HostIndexVM { GameId = service.GenerateGameId() };

            if (User.Identity.IsAuthenticated)
                return View(vm);
            else
                return RedirectToAction(nameof(AccountController.Login), nameof(AccountController));
        }

        [HttpPost]
        public IActionResult Index(string gameId)
        {
            sessionService.GameId = gameId;
            return RedirectToAction(nameof(HostGame));
        }

        public IActionResult HostGame(string gameId)
        {
            var vm = new HostGameVM { GameId = sessionService.GameId, SongIds = JsonConvert.SerializeObject(questionService.GetSongIds())};
            return View(vm);
        }

        public IActionResult ShowAlternatives(int song)
        {
            var alternatives = questionService.GetQuestionsForId(song);
            return PartialView("~/Views/Shared/Host/_Alternatives.cshtml", alternatives);
        }

        public IActionResult ShowResults()
        {
            return PartialView("~/Views/Shared/Host/_Results.cshtml");
        }

        public IActionResult ShowFinalResults()
        {
            return PartialView("~/Views/Shared/Host/_FinalResults.cshtml");
        }
    }
}