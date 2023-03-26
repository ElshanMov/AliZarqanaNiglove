using Core.Entities;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.ViewModels;

namespace App.Controllers
{
    public class BlogController : Controller
    {

        readonly IBlogRepository _blogRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IProductRepository _productRepository;
        readonly IBlogCommentRepository _blogCommentRepository;

        public BlogController(IBlogRepository blogRepository, ICategoryRepository categoryRepository, IProductRepository productRepository, IBlogCommentRepository blogCommentRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _blogCommentRepository = blogCommentRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            var blogs = await _blogRepository.GetAllAsync(x => !x.IsDeleted,false);
            var blog = await _blogRepository.GetAsync(x => x.Id == id,false);
            var categories = await _categoryRepository.GetAllAsync(x=>!x.IsDeleted,false,"Products");
            var blogComments = await _blogCommentRepository.GetAllAsync(x => !x.IsDeleted && x.BlogId == id);
            BlogViewModel model = new BlogViewModel
            {
                Blog = blog,
                Blogs = blogs.OrderByDescending(x=>x.CreatedAt).Take(4).ToList(),
                Categories = categories.ToList(),
                BlogComments = blogComments.ToList(),
                BlogComment = new BlogComment()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> sendComment(BlogViewModel comment)
        {

            if (comment.BlogComment.Text == null || comment.BlogComment.Text == null || comment.BlogComment.Name == null || comment.BlogComment.WebSite == null)
            {
                ModelState.AddModelError("", "all required");
                return RedirectToAction("detail", new { id = comment.BlogComment.BlogId });
            }

            comment.BlogComment.SendTime = DateTime.UtcNow.AddHours(4);

            await _blogCommentRepository.AddAsync(comment.BlogComment);
            await _blogCommentRepository.CommitAsync();

            return RedirectToAction("detail", new { id = comment.BlogComment.BlogId });
        }

    }
}
