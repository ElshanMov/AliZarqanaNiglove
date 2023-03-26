using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.CustomExceptions;
using Service.HelperServices.Interfaces;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("Adminpanel")]
    public class SettingController : Controller
    {
        readonly IWebHostEnvironment _env;
        readonly ISettingRepository _settingRepository;
        readonly IFileService _fileService;
        readonly ILocalService _localService;
        public SettingController(IWebHostEnvironment env, ISettingRepository settingRepository, IFileService fileService, ILocalService localService)
        {
            _env = env;
            _settingRepository = settingRepository;
            _fileService = fileService;
            _localService = localService;
        }
        public async Task<IActionResult> Index(int page = 1, string word = null)
        {
            var settings = await _settingRepository.GetAllAsync(x => true, false);
            return View(settings);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var setting = await _settingRepository.GetAsync(x => x.Id == id);

            if (setting == null) return RedirectToAction("notfound", "pages");

            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Settings entity)
        {
            if (!ModelState.IsValid) return View();

            var existEntity = await _settingRepository.GetAsync(x => x.Id == entity.Id);

            if (existEntity == null)
            {
                return RedirectToAction("notfound", "error");
            }

            if(entity.ImageFile != null)
            {
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
                await _localService.DeleteAsync(_env.WebRootPath, "uploads/settings", existEntity.Value);
                existEntity.Value = await _localService.SaveAsync(_env.WebRootPath, "uploads/settings", entity.ImageFile);

            }
            else
            {
                existEntity.Value = entity.Value;
            }

            await _settingRepository.CommitAsync();

            return RedirectToAction("index", "setting");
        }
    }
}
