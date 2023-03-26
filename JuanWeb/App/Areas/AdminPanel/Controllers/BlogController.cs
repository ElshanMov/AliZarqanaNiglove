using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.CustomExceptions;
using Service.HelperServices.Interfaces;
using System.Drawing.Drawing2D;
using System.Reflection.Metadata;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("Adminpanel")]
    public class BlogController : Controller
    {
        readonly IFileService _fileService;
        readonly ILocalService _localService;
        readonly IWebHostEnvironment _webHost;
        readonly IBlogRepository _blogRepository;
        public BlogController(IFileService fileService, ILocalService localService, IWebHostEnvironment webHost, IBlogRepository blogRepository)
        {
            _fileService = fileService;
            _localService = localService;
            _webHost = webHost;
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _blogRepository.GetAllAsync(x => x.IsDeleted == false);
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if(blog.Text == null)
            {
                ModelState.AddModelError("Text", "Required");
                return View();
            }else if(blog.Title == null)
            {
                ModelState.AddModelError("Title", "Required");
                return View();
            }

            try
            {
                _fileService.CheckFileLenght(blog.ImageFile.Length);
                _fileService.CheckImageFile(blog.ImageFile);
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

            blog.Image = await _localService.SaveAsync(_webHost.WebRootPath, "uploads/blogs/", blog.ImageFile);
            blog.CreatedAt = DateTime.UtcNow.AddHours(4);
            
            await _blogRepository.AddAsync(blog);
            await _blogRepository.CommitAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _blogRepository.GetAsync(x => x.Id == id);
            if (entity == null) return RedirectToAction("notfound", "error");
            return View(entity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Blog entity)
        {
            var existEntity = await _blogRepository.GetAsync(x => x.Id == entity.Id);

            if (existEntity == null)
            {
                return RedirectToAction("notfound", "error");
            }

            if (entity.Text == null)
            {
                ModelState.AddModelError("Text", "Required");
                return View();
            }
            else if (entity.Title == null)
            {
                ModelState.AddModelError("Title", "Required");
                return View();
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
                await _localService.DeleteAsync(_webHost.WebRootPath, "uploads/blogs", existEntity.Image);
                existEntity.Image = await _localService.SaveAsync(_webHost.WebRootPath, "uploads/blogs/", entity.ImageFile);
            }


            existEntity.Text = entity.Text;
            existEntity.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _blogRepository.CommitAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _blogRepository.GetAsync(x => x.Id == id);
            if (entity == null)
            {
                return RedirectToAction("notfound", "error");
            }
            entity.IsDeleted = true;
            await _blogRepository.CommitAsync();
            return Ok();
        }



    }
}
