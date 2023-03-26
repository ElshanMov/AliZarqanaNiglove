using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Service.DTOs;
using Service.Services.Interfaces;
using Service.ViewModels;

namespace App.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        IEmailService _emailService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User Name or Password incorrect");
                return View();
            }

            var result = _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false).Result;
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "User Name or Password incorrect");
                return View();
            }

            return RedirectToAction("index", "home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser user = new AppUser()
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            };
            await _userManager.AddToRoleAsync(user, "Member");


            return RedirectToAction("login", "account");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotDto forgotVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = await _userManager.FindByEmailAsync(forgotVM.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "User not found");
                return View();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("resetpassword", "account", new { email = user.Email, token = token }, Request.Scheme);

            _emailService.Send(forgotVM.Email, "Reset Password", url);

            return RedirectToAction("login", "account");
        }
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null || !(await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPasswordDto.Token)))
            {
                return RedirectToAction("login");
            }
            return View(resetPasswordDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ResetPasswordDto resetPasswordDto)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordDto.Password) || resetPasswordDto.Password.Length > 25)
            {
                ModelState.AddModelError("Password", "Password is required and must be less than 26 character");
            }

            if (!ModelState.IsValid)
            {
                return View("ResetPassword", resetPasswordDto);
            }

            AppUser user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user == null)
            {
                return RedirectToAction("login");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ResetPassword", resetPasswordDto);
            }

            return RedirectToAction("login", "account");
        }

        public async Task<IActionResult> Profile()
        {
            string UserCheck = User.Identity.Name;
            if (UserCheck == null)
            {
                return RedirectToAction("index", "home");
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ProfileViewModel profileVM = new ProfileViewModel
            {
                Member = new MemberUpdateDto
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    UserName = user.UserName
                },
                Orders = null
            };
            return View(profileVM);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> Profile(MemberUpdateDto memberVM)
        {
            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);
            ProfileViewModel profileVM = new ProfileViewModel
            {
                Member = memberVM,
                Orders = null
            };
            if (!ModelState.IsValid)
            {
                return View(profileVM);
            }
            if (!await _userManager.CheckPasswordAsync(member, memberVM.CurrentPassword))
            {
                ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct");
                return View(profileVM);
            }

            if (member.Email != memberVM.Email && _userManager.Users.Any(x => x.NormalizedEmail == memberVM.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "This email has already been taken");
                return View(profileVM);
            }
            if (member.UserName != memberVM.UserName && _userManager.Users.Any(x => x.NormalizedUserName == memberVM.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "This username has already been taken");
                return View(profileVM);
            }
            member.Email = memberVM.Email;
            member.FullName = memberVM.FullName;
            member.UserName = memberVM.UserName;
            var result = await _userManager.UpdateAsync(member);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(profileVM);
            }


            if (!string.IsNullOrWhiteSpace(memberVM.Password) && !string.IsNullOrWhiteSpace(memberVM.RepeatPassword))
            {
                if (memberVM.Password != memberVM.RepeatPassword)
                {
                    return View(profileVM);
                }
                var passwordResult = _userManager.ChangePasswordAsync(member, memberVM.CurrentPassword, memberVM.Password).Result;
                if (!passwordResult.Succeeded)
                {
                    foreach (var item in passwordResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(profileVM);
                }
            }
            await _signInManager.SignInAsync(member, true);
            return View(profileVM);
        }

    }
}
