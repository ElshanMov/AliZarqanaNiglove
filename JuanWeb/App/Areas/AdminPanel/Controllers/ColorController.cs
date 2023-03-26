using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]

    public class ColorController : Controller
    {
        readonly IColorRepository _ColorRepository;
        public ColorController(IColorRepository ColorRepository)
        {
            _ColorRepository = ColorRepository;
        }
        public async Task<IActionResult> Index(bool? deleted = null, string? search = null)
        {
            ICollection<Color> categor = await _ColorRepository.GetAllAsync(x => true, true);
            var categories = categor.AsQueryable();
            if (deleted == true)
            {
                categories = categories.Where(x => x.IsDeleted == true);
            }
            if (search != null)
            {
                categories = categories.Where(x => x.Name.Contains(search));
            }

            return View(categories.ToList());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Color Color)
        {
          
            await _ColorRepository.AddAsync(Color);
            await _ColorRepository.CommitAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            Color oldColor = await _ColorRepository.GetAsync(x => x.Id == id);
            if (oldColor == null)
            {
                return RedirectToAction("index", "error");
            }
            return View(oldColor);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Color Color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Color old = await _ColorRepository.GetAsync(x => x.Id == Color.Id);
            if (old == null)
            {
                return View(Color);
            }
            old.Name = Color.Name;
            await _ColorRepository.CommitAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int id)
        {
            Color oldColor = await _ColorRepository.GetAsync(x => x.Id == id);
            if (oldColor == null)
            {
                return RedirectToAction("index", "error");
            }
            if (oldColor.IsDeleted == false)
            {
                oldColor.IsDeleted = true;

            }
            else
            {
                oldColor.IsDeleted = false;
            }
            await _ColorRepository.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}
