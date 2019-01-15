using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuQuiz.Models;
using MuQuiz.Models.ViewModels;

namespace MuQuiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        AdminService service;
        public AdminController(AdminService service)
        {
            this.service = service;
        }

        [Route("/admin/index")]
        public IActionResult Index()
        {
            return Content("ADMINSIDA!!!!");
        }

        [HttpGet]
        public IActionResult Song()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Song(AdminSongVM vm, string submit)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if(submit == "Add")
            {
                await service.AddSong(vm);
                return RedirectToAction(nameof(Song));
            }

            return Content("Annat än add");
        }

        [HttpGet]
        public SongItem[] GetAllSongs()
        {
            return service.GetAllSongs();
        }
    }
}