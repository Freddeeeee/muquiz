using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuQuiz.Models;
using MuQuiz.Models.ViewModels;

namespace MuQuiz.Controllers
{
    [Authorize]
    public class HostController : Controller
    {
        HostService service;

        public HostController(HostService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = new HostIndexVM { GameId = service.GenerateGameId() };

            if (User.Identity.IsAuthenticated)
                return View(vm);
            else
                return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(int vm) //ändra parametertyp till vymodell
        {
            if (!ModelState.IsValid)
                return View(vm);

            //Skapa login-metoder i serviceklassen och validera inloggningen
            //var loginResult = await service.LoginAsync(vm);
            //if(loginResult.Succeeded)
            //  return RedirectToAction(nameof(Index));
            //else
            //{
            //  ModelState.AddModelError(nameof(VYMODELL.UserName), "Invalid user name and/or password.");
            //  return View(vm);
            //}

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Index(string gameId)
        {
            HttpContext.Session.SetString("GameId", gameId);
            return RedirectToAction(nameof(HostGame));
        }

        public IActionResult HostGame(string gameId)
        {
            var vm = new HostGameVM { GameId = HttpContext.Session.GetString("GameId") };
            return View(vm);
        }

        public IActionResult ShowAlternatives(string song)
        {
            // to-do: retrieve alternatives from data storage
            return PartialView("~/Views/Shared/Host/_ShowAlternatives.cshtml");
        }

        public IActionResult ShowResults()
        {
            return PartialView("~/Views/Shared/Host/_ShowResults.cshtml");
        }

        public IActionResult ShowFinalResults()
        {
            return PartialView("~/Views/Shared/Host/_ShowFinalResults.cshtml");
        }
    }
}