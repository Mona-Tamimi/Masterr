using Microsoft.AspNetCore.Mvc;

namespace master.Controllers
{
    public class consultantController : Controller
    {
        public IActionResult consultant()
        {
            return View();
        }

        public IActionResult consultantProfile()
        {
            return View();
        }
    }
}
