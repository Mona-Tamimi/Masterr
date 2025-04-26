using System.Diagnostics;
using master.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace master.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;

        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // ? GET: Contact Page
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        // ? POST: Submit Contact Form
        [HttpPost]
        public IActionResult Contact(ContactMessage model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please check the form fields.";
                return View(model);
            }

            _context.ContactMessages.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Your message has been sent successfully!";
            return RedirectToAction("Contact");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
