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
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string GameId)
        {
            HttpContext.Session.SetString("GameId", GameId);
            return RedirectToAction(nameof(PlayerController.Index), "Player");
        }
    }
}