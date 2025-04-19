using Microsoft.AspNetCore.Mvc;

namespace CRUD_asp.netMVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
