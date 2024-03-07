using AutomationSystem.Areas.Identity.Data;
using AutomationSystem.Data;
using AutomationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AutomationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AutomationSystemContext _context;

        public HomeController(ILogger<HomeController> logger, AutomationSystemContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Master"))
            {
                return RedirectToAction("Index", "Master");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Categories()
        {
            var Categories = _context.FixCategories.ToList();
            return View(Categories);
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