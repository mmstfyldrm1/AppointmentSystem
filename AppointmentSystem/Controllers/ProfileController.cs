using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Controllers
{
    public class ProfileController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
