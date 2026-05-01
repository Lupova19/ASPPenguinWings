using ASPPenguinWings.Data;
using ASPPenguinWings.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASPPenguinWings.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //кл
        //private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        //{
        //    _logger = logger;
        //    _context = context;
        //}
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(string name, string email, string message)
        {
            // тук може после да запазваш в база или пращаш имейл

            TempData["Success"] = "Съобщението беше изпратено успешно!";

            return RedirectToAction("Contact");
        }
    }
}
