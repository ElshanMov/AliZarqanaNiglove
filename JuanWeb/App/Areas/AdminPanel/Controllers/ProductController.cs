using Core.Entities;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.CustomExceptions;
using Service.HelperServices.Implementations;
using Service.HelperServices.Interfaces;
using System.Drawing;

namespace App.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]

    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ICategoryRepository _c;
        private readonly IProductImagesRepository _images;




        readonly ILocalService _localService;
        readonly IFileService _fileService;
        readonly IWebHostEnvironment _webHost;
        public ProductController(IProductRepository productRepository, IProductImagesRepository productImages , ICategoryRepository categoryRepository, IColorRepository colorRepository, ISizeRepository sizeRepository,ILocalService localService,IFileService fileService,IWebHostEnvironment webHost)
        {
            _productRepository= productRepository;
            _colorRepository= colorRepository;
            _c= categoryRepository;
            _localService = localService;
            _sizeRepository= sizeRepository;    
            _webHost = webHost;
            _fileService = fileService;
            _images = productImages;
        }
        public async Task<IActionResult> Index(int page=1,int categoryId=0,string? search=null)
        {
            ICollection<Product> products = await _productRepository.GetAllAsync(x => true, false, "ProductImages", "Category");
            
            if (search != null)
            {
                products = await _productRepository.GetAllAsync(x => x.Name.Contains(search), false);
            }
            if (categoryId != 0)
            {
                products=await _productRepository.GetAllAsync(x=>x.CategoryId==categoryId, false);
            }
            return View(products.ToList());
        }
        public async Task<IActionResult> Create()
        {
            var size = await _sizeRepository.GetAllAsync(x => x.IsDeleted == false);
            var color = await _colorRepository.GetAllAsync(x => x.IsDeleted == false);
            var categor = await _c.GetAllAsync(x => x.IsDeleted == false);

            ViewBag.SizeIds = size.ToList();
            ViewBag.ColorIds = color.ToList();
            ViewBag.CategoryIds = categor.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var size = await _sizeRepository.GetAllAsync(x => x.IsDeleted == false);
            var color = await _colorRepository.GetAllAsync(x => x.IsDeleted == false);
            var categor = await _c.GetAllAsync(x => x.IsDeleted == false);

            ViewBag.SizeIds = size.ToList();
            ViewBag.ColorIds = color.ToList();
            ViewBag.CategoryIds = categor.ToList();
            product.Sizes = new List<ProductSizes>();
            foreach (var sizeId in product.SizeIds)
            {
 
                ProductSizes productSizes = new ProductSizes
                {
                    SizeId = sizeId,
                    Product = product
                };
                product.Sizes.Add(productSizes);
            }
            if (product.PosterImage == null)
            {
                ModelState.AddModelError("PosterImage", "Poster şəkli boş ola bilməz!");
                return View();
            }
            else
            {
                product.ProductImages = new List<ProductImage>();
                try
                {
                    _fileService.CheckFileLenght(product.PosterImage.Length);
                    _fileService.CheckImageFile(product.PosterImage);
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
               string filename = await _localService.SaveAsync(_webHost.WebRootPath, "uploads/products", product.PosterImage);
                ProductImage productImage = new ProductImage
                {
                    Image = filename,
                    IsPoster = true
                };
                product.ProductImages.Add(productImage);
            }
            if(product.ImageFiles!=null)
            {
                foreach (var item in product.ImageFiles)
                {
                    try
                    {
                        _fileService.CheckFileLenght(product.PosterImage.Length);
                        _fileService.CheckImageFile(product.PosterImage);
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
                    string filename = await _localService.SaveAsync(_webHost.WebRootPath, "uploads/products", product.PosterImage);
                    ProductImage productImage = new ProductImage
                    {
                        Image = filename,
                        IsPoster = false
                    };
                    product.ProductImages.Add(productImage);
                }
            }
            if (product.SalePrice < 0)
            {
                ModelState.AddModelError("Price", "Qiymət mənfi ola bilməz");
                return View();

            }
            if (product.CostPrice < 0)
            {
                ModelState.AddModelError("Price", "Qiymət mənfi ola bilməz");
                return View();

            }
            if (product.DiscountPrice < 0)
            {
                ModelState.AddModelError("Price", "Qiymət mənfi ola bilməz");
                return View();

            }
            if (product.InStock < 0)
            {
                ModelState.AddModelError("Price", "Say mənfi ola bilməz");
                return View();

            }
            if(product.Information != null)
            {
                ModelState.AddModelError("Information", "Say mənfi ola bilməz");
                return View();
            }



            await _productRepository.AddAsync(product);
            await _productRepository.CommitAsync();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var size = await _sizeRepository.GetAllAsync(x => x.IsDeleted == false);
            var color = await _colorRepository.GetAllAsync(x => x.IsDeleted == false);
            var categor = await _c.GetAllAsync(x => x.IsDeleted == false);

            ViewBag.SizeIds = size.ToList();
            ViewBag.ColorIds = color.ToList();
            ViewBag.CategoryIds = categor.ToList();

            Product product=await _productRepository.GetAsync(x=>x.Id== id,true,"ProductImages","Category", "Sizes");
            product.SizeIds = product.Sizes.Select(x => x.SizeId).ToList();

            if (product==null) {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            var size = await _sizeRepository.GetAllAsync(x => x.IsDeleted == false);
            var color = await _colorRepository.GetAllAsync(x => x.IsDeleted == false);
            var categor = await _c.GetAllAsync(x => x.IsDeleted == false);

            ViewBag.SizeIds = size.ToList();
            ViewBag.ColorIds = color.ToList();
            ViewBag.CategoryIds = categor.ToList();
            Product old=await _productRepository.GetAsync(x=>x.Id==product.Id,true,"ProductImages","Category", "Sizes");
            if (old == null)
            {
                return NotFound();
            }

            old.Sizes.RemoveAll(x => !product.SizeIds.Contains(x.SizeId));
            foreach (var colorId in product.SizeIds.Where(x => !old.Sizes.Any(bt => bt.SizeId == x)))
            {
                ProductSizes productColor = new ProductSizes
                {
                    ProductId = product.Id,
                    SizeId = colorId
                };

                old.Sizes.Add(productColor);
            }
            if (product.PosterImage != null)
            {
                try
                {
                    _fileService.CheckFileLenght(product.PosterImage.Length);
                    _fileService.CheckImageFile(product.PosterImage);
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
                ProductImage posterImage =await _images.GetAsync(x => x.ProductId == product.Id && x.IsPoster == true);
                string filename =await _localService.SaveAsync(_webHost.WebRootPath, "uploads/products", product.PosterImage);
                if (posterImage == null)
                {
                    posterImage = new ProductImage
                    {
                        IsPoster = true,
                        Image = filename,
                        ProductId = product.Id
                    };
                   await _images.AddAsync(posterImage);
                }
                else
                {
                   await _localService.DeleteAsync(_webHost.WebRootPath, "uploads/products", posterImage.Image);
                    posterImage.Image = filename;
                }
            }
            await _images.CommitAsync();
            old.ProductImages.RemoveAll(x => x.IsPoster == false && !product.ProductImageIds.Contains(x.Id));

            if (product.ImageFiles != null)
            {
                foreach (var file in product.ImageFiles)
                {
                    product.ProductImages = new List<ProductImage>();
                    try
                    {
                        _fileService.CheckFileLenght(file.Length);
                        _fileService.CheckImageFile(file);
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
                    ProductImage image = new ProductImage
                    {
                        IsPoster = false,
                        Image =await _localService.SaveAsync(_webHost.WebRootPath, "uploads/products", file)
                    };
                    if (old.ProductImages == null)
                        old.ProductImages = new List<ProductImage>();
                    old.ProductImages.Add(image);
                }
            }
            
            old.Name = product.Name;
            old.Information = product.Information;
            old.SalePrice = product.SalePrice;
            old.CostPrice = product.CostPrice;
            old.DiscountPrice = product.DiscountPrice;
            old.InStock = product.InStock;
            old.ColorId = product.ColorId;
            old.Description = product.Description;
            old.CategoryId = product.CategoryId;
            await _productRepository.CommitAsync();
            return RedirectToAction("index");
        }
		public async Task<IActionResult> Delete(int id)
		{
			Product oldcategory = await _productRepository.GetAsync(x => x.Id == id);
			if (oldcategory == null)
			{
				return RedirectToAction("index", "error");
			}
			if (oldcategory.IsDeleted == false)
			{
				oldcategory.IsDeleted = true;

			}
			else
			{
				oldcategory.IsDeleted = false;
			}
			await _productRepository.CommitAsync();
			return RedirectToAction("Index");
		}

	}
}
