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

        public IActionResult HostGame()
        {
            var gameId = service.GenerateGameId();
            sessionService.GameId = gameId;

            var vm = new HostGameVM
            {
                GameId = gameId,
                SongIds = JsonConvert.SerializeObject(questionService.GetSongIds()),
                SpotifyToken = spotify.Token.AccessToken,
            };

            return View(vm);
        }

        public IActionResult ShowAlternatives(int song)
        {
            var alternatives = questionService.GetQuestionsForId(song);
            return PartialView("~/Views/Shared/Host/_Alternatives.cshtml", alternatives);
        }

        public async Task<IActionResult> ShowResults()
        {
            var players = await gameService.GetAllPlayers(sessionService.GameId);
            return PartialView("~/Views/Shared/Host/_Results.cshtml", players);
        }

        public async Task<IActionResult> ShowFinalResults()
        {
            //await gameService.StopPlaying(sessionService.GameId);
            var players = await gameService.GetAllPlayers(sessionService.GameId);
            return PartialView("~/Views/Shared/Host/_FinalResults.cshtml", players);
        }

        public IActionResult GetSpotifyId(int id)
        {
            return Json(questionService.GetSpotifyId(id));
        }
    }
}