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
    public class HostController : Controller
    {
        private readonly HostService service;
        private readonly SessionStorageService sessionService;


        public HostController(HostService service, SessionStorageService sessionStorageService)
        {
            this.service = service;
            sessionService = sessionStorageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = new HostIndexVM { GameId = service.GenerateGameId() };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(string gameId)
        {
            sessionService.GameId = gameId;
            return RedirectToAction(nameof(HostGame));
        }

        public IActionResult HostGame(string gameId)
        {
            var vm = new HostGameVM { GameId = sessionService.GameId };
            return View(vm);
        }

        public IActionResult ShowAlternatives(string song)
        {
            // to-do: retrieve alternatives from data storage
            return PartialView("~/Views/Shared/Host/_Alternatives.cshtml");
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