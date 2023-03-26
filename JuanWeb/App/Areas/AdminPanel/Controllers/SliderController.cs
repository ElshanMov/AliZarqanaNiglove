using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Service.CustomExceptions;
using Service.HelperServices.Interfaces;
using Service.Services.Interfaces;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderController : Controller
    {
        readonly ISliderService _sliderService;
        readonly ILocalService _localService;
        readonly IFileService _fileService;
        readonly IWebHostEnvironment _webHost;
        public SliderController(ISliderService sliderService, ILocalService localService, IWebHostEnvironment webHost, IFileService fileService)
        {
            _sliderService = sliderService;
            _localService = localService;
            _webHost = webHost;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var data=await _sliderService.GetAllAsync(x=>x.IsDeleted==false);
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        //todo Must be refactor
        public async Task<IActionResult> Create(Slider slider)
        {
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Şəkil boş buraxıla bilməz.");
                return View();
            }
          
            try
            {
                _fileService.CheckFileLenght(slider.ImageFile.Length);
                _fileService.CheckImageFile(slider.ImageFile);
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




            slider.Image = await _localService.SaveAsync(_webHost.WebRootPath, "uploads/sliders", slider.ImageFile);
            await _sliderService.AddAsync(slider);
            await _sliderService.CommitAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Slider slider=await _sliderService.GetAsync(x=>x.Id== id);
            if (slider==null)
            {
                return RedirectToAction("notfound", "error");
            }
            slider.IsDeleted = true;
            await _localService.DeleteAsync(_webHost.WebRootPath, "uploads/sliders", slider.Image);
            await _sliderService.CommitAsync();
            return Ok();
        }
        public async Task<IActionResult> Edit(int id)
        {
            Slider slider = await _sliderService.GetAsync(x => x.Id == id);
            if (slider == null) return RedirectToAction("notfound","error");
            return View(slider);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> Edit(Slider slider) 
        {
            Slider existSlider = await _sliderService.GetAsync(x => x.Id == slider.Id);
            if (existSlider==null)
            {
                return RedirectToAction("notfound", "error");
            }
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Şəkil boş buraxıla bilməz.");
            }
            else if (slider.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "Şəkilin ölçüsü 2MB-dan artıq ola bilməz.");
            }
            else if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "Fayl Tipi düzgün deyil");
            }
            await _localService.DeleteAsync(_webHost.WebRootPath, "uploads/sliders", existSlider.Image);
            existSlider.Image = await _localService.SaveAsync(_webHost.WebRootPath, "uploads/sliders", slider.ImageFile);
            existSlider.Title = slider.Title;
            existSlider.SecondTitle = slider.SecondTitle;
            existSlider.Description = slider.Description;
            await _sliderService.CommitAsync();
            return RedirectToAction("index");

        }

    }
}
