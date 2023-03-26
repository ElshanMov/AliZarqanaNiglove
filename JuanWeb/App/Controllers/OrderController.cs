using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult Compare()
        {
            return View();
        }
    }
}
