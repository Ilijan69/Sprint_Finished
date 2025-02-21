using Categories2024.Data;
using Categories2024.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Categories2024.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            _context = db;
        }

        public async Task <IActionResult> Index()
        {
            return View(_context.Categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj != null)
            {
                _context.Add(obj);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
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
    }
}
