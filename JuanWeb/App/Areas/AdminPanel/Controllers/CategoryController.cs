using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository=categoryRepository;
        }
        public async Task<IActionResult> Index(bool? deleted= null,string? search = null)
        {
            ICollection<Category> categor =await _categoryRepository.GetAllAsync(x=>true,true);
            var categories = categor.AsQueryable();
            if(deleted==true)
            {
                categories=categories.Where(x => x.IsDeleted == true);
            }
            if(search!=null)
            {
                categories=categories.Where(x=>x.Name.Contains(search));
            }
   
            return View(categories.ToList());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            Category oldcategory =await _categoryRepository.GetAsync(x => x.Id == id);
            if (oldcategory == null)
            {
                return RedirectToAction("index", "error");
            }
            return View(oldcategory);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            Category old=await _categoryRepository.GetAsync(x=>x.Id==category.Id);    
            if(old==null)
            {
                return View(category);
            }
            old.Name=category.Name;
            await _categoryRepository.CommitAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int id)
        {
            Category oldcategory = await _categoryRepository.GetAsync(x => x.Id == id);
            if (oldcategory == null)
            {
                return RedirectToAction("index", "error");
            }
            if(oldcategory.IsDeleted==false)
            {
                oldcategory.IsDeleted = true;

            }
            else
            {
                oldcategory.IsDeleted=false;
            }
            await _categoryRepository.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}
