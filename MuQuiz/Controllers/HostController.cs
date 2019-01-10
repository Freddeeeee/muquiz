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
        private readonly GameService gameService;
        SessionStorageService sessionService;

        public HostController(HostService service, SessionStorageService sessionStorageService, SpotifyService spotify, QuestionService questionService, GameService gameService)
        {
            this.service = service;
            sessionService = sessionStorageService;
            this.spotify = spotify;
            this.questionService = questionService;
            this.gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var vm = new HostIndexVM { GameId = service.GenerateGameId() };
                await gameService.InitializeSession(vm.GameId);
                return View(vm);
            }
            else
                return RedirectToAction(nameof(AccountController.Login), nameof(AccountController));
        }

        //SCH: change to using sessionStorage instead of hidden field?
        public async Task<IActionResult> HostGame(string gameId)
        {
            await gameService.StartPlaying(gameId);
            var vm = new HostGameVM { GameId = gameId, SongIds = JsonConvert.SerializeObject(questionService.GetSongIds())};
            return View(vm);
        }

        public IActionResult ShowAlternatives(int song)
        {
            var alternatives = questionService.GetQuestionsForId(song);
            return PartialView("~/Views/Shared/Host/_Alternatives.cshtml", alternatives);
        }

        public IActionResult ShowResults()
        {
            var players = gameService.GetAllPlayers(sessionService.GameId);
            return PartialView("~/Views/Shared/Host/_Results.cshtml", players);
        }

        public IActionResult ShowFinalResults()
        {
            var players = gameService.GetAllPlayers(sessionService.GameId);
            return PartialView("~/Views/Shared/Host/_FinalResults.cshtml", players);
        }
    }
}