using Microsoft.AspNetCore.Identity;
using MuQuiz.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class AccountService
    {
        UserManager<MyIdentityUser> userManager;
        SignInManager<MyIdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;

        public AccountService(
            UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        internal async Task<IdentityResult> createAccount(AccountLoginVM vm)
        {
            return await userManager.CreateAsync(
                new MyIdentityUser { UserName = vm.UserName }, vm.Password);
        }

        internal async Task<SignInResult> LoginAsync(AccountLoginVM vm)
        {
            return await signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
        }

        internal async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
