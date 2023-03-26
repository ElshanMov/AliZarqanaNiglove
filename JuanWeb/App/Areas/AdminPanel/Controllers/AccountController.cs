using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.DTOs;
using Service.Services.Interfaces;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AccountController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x=> x.Email==loginVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User Name or Password incorrect");
                return View();
            }

            var result = _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false).Result;
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "User Name or Password incorrect");
                return View();
            }

            return RedirectToAction("index", "dashboard");
        }

        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login","account");
        }

        
    }
}
