using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SizeController : Controller
    {
        readonly ISizeRepository _SizeRepository;
        public SizeController(ISizeRepository SizeRepository)
        {
            _SizeRepository = SizeRepository;
        }
        public async Task<IActionResult> Index(bool? deleted = null, string? search = null)
        {
            ICollection<Size> categor = await _SizeRepository.GetAllAsync(x => true, true);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Size Size)
        {
            await _SizeRepository.AddAsync(Size);
            await _SizeRepository.CommitAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            Size oldSize = await _SizeRepository.GetAsync(x => x.Id == id);
            if (oldSize == null)
            {
                return RedirectToAction("index", "error");
            }
            return View(oldSize);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Size Size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Size old = await _SizeRepository.GetAsync(x => x.Id == Size.Id);
            if (old == null)
            {
                return View(Size);
            }
            old.Name = Size.Name;
            await _SizeRepository.CommitAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int id)
        {
            Size oldSize = await _SizeRepository.GetAsync(x => x.Id == id);
            if (oldSize == null)
            {
                return RedirectToAction("index", "error");
            }
            if (oldSize.IsDeleted == false)
            {
                oldSize.IsDeleted = true;

            }
            else
            {
                oldSize.IsDeleted = false;
            }
            await _SizeRepository.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}
