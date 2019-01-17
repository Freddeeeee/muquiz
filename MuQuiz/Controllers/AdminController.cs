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

        [HttpGet("/admin")]
        public async Task<IActionResult> Index()
        {
            var allSongs = await service.GetAllSongs();
            return View(allSongs.OrderBy(s => s.Artist).ToArray());
        }

        [HttpGet("/admin/add")]
        public IActionResult AddSong()
        {
            return View();
        }

        [HttpPost("/admin/add")]
        public async Task<IActionResult> AddSong(AdminAddEditSongVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await service.AddSong(vm);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/admin/update/{id}")]
        public async Task<IActionResult> UpdateSong(int id)
        {
            var vm = await service.GetSongForUpdate(id);
            return View(vm);
        }

        [HttpPost("/admin/update")]
        public async Task<IActionResult> UpdateSong(AdminAddEditSongVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await service.UpdateSong(vm);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("/admin/delete/{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            await service.DeleteSong(id);
            return RedirectToAction(nameof(Index));
        }
    }
}