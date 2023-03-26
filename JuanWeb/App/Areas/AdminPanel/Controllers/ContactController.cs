using Core.Entities;
using Core.Enums;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;
        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<IActionResult> Index(int id=-1)
        {
            ICollection<Contact> contacts =await _contactRepository.GetAllAsync(x => true,true);
            if(id!=-1)
            {
                contacts=contacts.Where(x=>x.Status==id).ToList();
            }
            return View(contacts.OrderBy(x => x.Id).Reverse().ToList());
        }
        public async Task<IActionResult> ChangeSituation(int id)
        {
            Contact contact =await _contactRepository.GetAsync(x => x.Id == id);
            if(contact == null)
            {
                return NotFound("Error");
            }
            if(contact.Status==Convert.ToInt32(ContactStatus.Baxilmayib))
            {
                contact.Status=Convert.ToInt32(ContactStatus.Baxilib);
            }
            else
            {
				contact.Status = Convert.ToInt32(ContactStatus.Cavablandi);
			}
             await _contactRepository.CommitAsync();
            return RedirectToAction("index");
        }

	}
}
