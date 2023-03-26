using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.CustomExceptions;
using Service.HelperServices.Interfaces;
using System.Drawing.Drawing2D;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BrandController : Controller
    {
            private readonly IBrandRepository _brandRepository;
            readonly IFileService _fileService;
            readonly ILocalService _localService;
            readonly IWebHostEnvironment _webHost;
            public BrandController(IBrandRepository brandRepository, IFileService fileService, ILocalService localService, IWebHostEnvironment webHost)
            {
                _brandRepository = brandRepository;
                _fileService = fileService;
                _localService = localService;
                _webHost = webHost;
            }

            public async Task<IActionResult> Index()
            {
                var list = await _brandRepository.GetAllAsync(x => x.IsDeleted == false);
                return View(list);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Brand brand)
            {

                try
                {
                    _fileService.CheckFileLenght(brand.ImageFile.Length);
                    _fileService.CheckImageFile(brand.ImageFile);
                }
                catch (ImageFileLenghtException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View();
                }
                catch (CheckImageFileTypeException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View();
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }

                brand.Image = await _localService.SaveAsync(_webHost.WebRootPath, "uploads/brands", brand.ImageFile);

                await _brandRepository.AddAsync(brand);
                await _brandRepository.CommitAsync();

                return RedirectToAction("Index");
            }

            public async Task<IActionResult> Edit(int id)
            {
                var entity = await _brandRepository.GetAsync(x => x.Id == id);
                if (entity == null) return RedirectToAction("notfound", "error");
                return View(entity);
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(Brand entity)
            {
                var existEntity = await _brandRepository.GetAsync(x => x.Id == entity.Id);

                if (existEntity == null)
                {
                    return RedirectToAction("notfound", "error");
                }

                try
                {
                    _fileService.CheckFileLenght(entity.ImageFile.Length);
                    _fileService.CheckImageFile(entity.ImageFile);
                }
                catch (ImageFileLenghtException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View();
                }
                catch (CheckImageFileTypeException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View();
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }


                await _localService.DeleteAsync(_webHost.WebRootPath, "uploads/brands", existEntity.Image);
                existEntity.Image = await _localService.SaveAsync(_webHost.WebRootPath, "uploads/sliders", entity.ImageFile);
                await _brandRepository.CommitAsync();
                return RedirectToAction("index");
            }

            public async Task<IActionResult> Delete(int id)
            {
                var entity= await _brandRepository.GetAsync(x => x.Id == id);
                if (entity == null)
                {
                    return RedirectToAction("notfound", "error");
                }
                entity.IsDeleted = true;
                await _localService.DeleteAsync(_webHost.WebRootPath, "uploads/brands", entity.Image);
                await _brandRepository.CommitAsync();
                return Ok();
            }



    }
}
