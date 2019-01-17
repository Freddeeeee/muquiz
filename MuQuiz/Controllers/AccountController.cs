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
    public class AccountController : Controller
    {
        AccountService service;

        public AccountController(AccountService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new AccountLoginVM { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var loginResult = await service.LoginAsync(vm);

            if (loginResult.Succeeded)
            {
                if (!string.IsNullOrEmpty(vm.ReturnUrl))
                    return Redirect(vm.ReturnUrl);
                else
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ModelState.AddModelError(nameof(AccountLoginVM.UserName), "Invalid username and/or password.");
                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountLoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            else
            {
                var createResult = await service.createAccount(vm);
                if (createResult.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                {
                    ModelState.AddModelError(nameof(AccountLoginVM.UserName), "Invalid username");
                    return View(vm);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // To-do: lägg till logga ut-knappar i host-vyerna
            await service.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}