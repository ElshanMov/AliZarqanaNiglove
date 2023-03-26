using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class ContactController : Controller
    {
        IContactRepository _contactRepository;
        UserManager<AppUser> _userManager;
        public ContactController(IContactRepository contactRepository, UserManager<AppUser> userManager)
        {
            _contactRepository = contactRepository;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            AppUser member=null;
            if(User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }
            if(member !=null)
            {
                Contact contact = new Contact();
                contact.Name = member.UserName;
                contact.Email = member.Email;
                contact.Phone = member.PhoneNumber;
                return View(contact);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Contact contact)
        {
           if(!ModelState.IsValid)
            {
                return View(contact);
            }
            await _contactRepository.AddAsync(contact);
            await _contactRepository.CommitAsync();
            return RedirectToAction("index");
        }
    }
}
