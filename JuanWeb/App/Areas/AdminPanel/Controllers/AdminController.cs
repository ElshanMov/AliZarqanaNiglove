using Core.Entities;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.DTOs;
using Service.Services.Interfaces;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AdminController : Controller
    {
        //***Serviceler işləmək üçün hazir deyil***
        IRoleService _roleService;
        IUserService _userService;
        UserManager<AppUser> _userManager;
        DataContext _context;
        public AdminController(IRoleService roleService, IUserService userService,UserManager<AppUser> userManager,DataContext context)
        {
            _roleService = roleService;
            _userService = userService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var data =  await _userService.GetAll();
            ViewBag.Users = _userManager;
            return View(data);
        }

        public IActionResult CreateRole() {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateRole(string name) {
            var data=await _roleService.CreateRole(name);
            return RedirectToAction("index");
        }

        public IActionResult CreateUser()
        {
            ViewBag.Role=_roleService.GetAllRoles().ToList();
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUser createUser)
        {
            ViewBag.Role = _roleService.GetAllRoles().ToList();

            if (createUser==null)
                return NotFound();
            AppUser appUser = new AppUser
            {
                FullName = createUser.FullName,
                UserName = createUser.Username,
                Email = createUser.Email
            };
            var result = await _userManager.CreateAsync(appUser, createUser.Password);
            await _userManager.AddToRolesAsync(appUser,createUser.Roles);
            return RedirectToAction("index", "admin");
        }

        public async Task<IActionResult> EditUser(string id) {
            ViewBag.Role = _roleService.GetAllRoles().ToList();

            var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id==id);
            var roles=await _userManager.GetRolesAsync(user);
            var data = new CreateUser
            {
                Email = user.Email,
                FullName = user.FullName,
                Username = user.UserName,
                Roles=roles
            };
            
            return View(data);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditUser(CreateUser user)
        {
            var existUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == user.Username);
            existUser.UserName = user.Username;
            existUser.Email = user.Email;
            existUser.FullName = user.FullName;            
            await _userService.AssignRoleToUserAsnyc(existUser.Id, user.Roles.ToArray());
            if (!string.IsNullOrEmpty(user.Password))
            {
                await _userService.UpdatePasswordAsync(existUser.Id, user.Password);
            }
            return RedirectToAction("index");
        }

    }
}
