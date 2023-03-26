using Core.Entities;
using Data;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service.DTOs;
using Service.ViewModels;
using System.Linq.Expressions;

namespace App.Controllers
{
    public class ProductController : Controller
    {

        readonly IProductRepository _productRepository;
        readonly IProductCommentRepository _productCommentRepository;
        DataContext _context;
        UserManager<AppUser> _userManager;
        public ProductController(IProductRepository productRepository, IProductCommentRepository productCommentRepository,DataContext context,UserManager<AppUser> userManager)
        {
            _productRepository = productRepository;
            _productCommentRepository = productCommentRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, string? word = null)
        {
            ICollection<Product> products = await _productRepository.GetAllAsync(x => true, false, "ProductImages","Category", "ProductComments");
            if (word != null)
            {
                products = await _productRepository.GetAllAsync(x => x.Name.Contains(word), false);
            }
            PagenatedListDto<Product> list  = PagenatedListDto<Product>.Save(products.AsQueryable(), page,1);
            return View(list);
        }   
        public async Task<IActionResult> Detail(int id)
        {
            var relatedPro = await _productRepository.GetAllAsync(x => x.Id != id,false, "ProductImages");
            ProductViewModel model = new()
            {
                Product = await _productRepository.GetAsync(x => x.Id == id, false, "ProductImages", "ProductComments"),
                ProductComment = new ProductComment(),
                RelatedProduct = relatedPro.Take(6).ToList()
            };

            return View(model);
        }
       
        public IActionResult Wishlist()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> sendComment(ProductViewModel comment)
        {

            if(comment.ProductComment.Text == null || comment.ProductComment.FullName == null)
            {
                ModelState.AddModelError("","xeta bas verdi");
                return RedirectToAction("detail", new { id = comment.ProductComment.ProductId });
            }

            comment.ProductComment.SendTime = DateTime.UtcNow.AddHours(4);

            await _productCommentRepository.AddAsync(comment.ProductComment);
            await _productCommentRepository.CommitAsync();

            return RedirectToAction("detail", new { id = comment.ProductComment.ProductId });
        }
        public async Task<IActionResult> AddBasket(int productId, int count = 1)
        {
            if (!_context.Products.Any(x => x.Id == productId))
            {
                return RedirectToAction("errorpage", "pages");
            }

            BasketViewModel data = null;

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            if (user != null && !user.IsAdmin)
            {

                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.AppUserId == user.Id && x.ProductId == productId);
                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = user.Id,
                        ProductId = productId,

                        Count = count > 1 ? count : 1
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count += count;
                }
                _context.SaveChanges();

                data = _getBasketItems(_context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == user.Id).ToList());

            }
            else
            {
                List<CookieBasketItemViewModel> basketItems = new List<CookieBasketItemViewModel>();

                string existBasketItems = HttpContext.Request.Cookies["basketItemList"];

                if (existBasketItems != null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(existBasketItems);

                }
                CookieBasketItemViewModel item = basketItems.FirstOrDefault(x => x.ProductId == productId);
                if (item == null)
                {
                    item = new CookieBasketItemViewModel
                    {
                        ProductId = productId,
                        Count = count > 1 ? count : 1
                    };
                    basketItems.Add(item);
                }
                else
                {
                    item.Count += count;
                }

                var productIdStr = JsonConvert.SerializeObject(basketItems);

                HttpContext.Response.Cookies.Append("basketItemList", productIdStr);

                data = _getBasketItems(basketItems);
            }
            return PartialView("_BasketItemViewPartial", data);
        }
        private BasketViewModel _getBasketItems(List<BasketItem> basketItems)
        {
            BasketViewModel basket = new BasketViewModel
            {
                BasketItems = new List<BasketItemViewModel>(),
            };

            foreach (var item in basketItems)
            {

                BasketItemViewModel basketItem = new BasketItemViewModel
                {
                    Name = item.Product.Name,
                    Price = item.Product.DiscountPrice > 0 ? (item.Product.SalePrice * (1 - item.Product.DiscountPrice / 100)) : item.Product.SalePrice,
                    ProductId = item.Product.Id,
                    Count = item.Count,
                    PosterImage = item.Product.ProductImages.FirstOrDefault(x => x.IsPoster == true)?.Image
                };

                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.BasketItems.Add(basketItem);
            }

            return basket;
        }
        private BasketViewModel _getBasketItems(List<CookieBasketItemViewModel> cookieBasketItems)
        {
            BasketViewModel basket = new BasketViewModel
            {
                BasketItems = new List<BasketItemViewModel>(),
            };

            foreach (var item in cookieBasketItems)
            {
                Product product = _context.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId);
                BasketItemViewModel basketItem = new BasketItemViewModel
                {
                    Name = product.Name,
                    Price = product.DiscountPrice > 0 ? (product.SalePrice * (1 - product.DiscountPrice / 100)) : product.SalePrice,
                    ProductId = product.Id,
                    Count = item.Count,
                    PosterImage = product.ProductImages.FirstOrDefault(x => x.IsPoster == true)?.Image
                };

                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.BasketItems.Add(basketItem);
            }

            return basket;
        }
        public async Task<IActionResult> DeleteFromBasket(int id)
        {
            List<CookieBasketItemViewModel> cookieBaskets = new List<CookieBasketItemViewModel>();
            AppUser user = null;
            BasketViewModel data = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            if (user != null && !user.IsAdmin)
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.ProductId == id);
                if (basketItem.Count == 1)
                {
                    _context.BasketItems.Remove(basketItem);
                }
                else
                {
                    basketItem.Count--;
                }
                _context.SaveChanges();
                data = _getBasketItems(_context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == user.Id).ToList());
            }
            else
            {
                string basketStr = HttpContext.Request.Cookies["basketItemList"];
                cookieBaskets = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(basketStr);
                CookieBasketItemViewModel cookieBasket = cookieBaskets.FirstOrDefault(x => x.ProductId == id);
                if (cookieBasket == null) return NotFound();

                if (cookieBasket.Count < 2)
                {
                    cookieBaskets.Remove(cookieBasket);
                }
                else
                {
                    cookieBasket.Count--;
                }
                data = _getBasketItems(cookieBaskets);
                HttpContext.Response.Cookies.Append("basketItemList", JsonConvert.SerializeObject(cookieBaskets));

            }
            return PartialView("_BasketItemViewPartial", data);
        }



    }
}
