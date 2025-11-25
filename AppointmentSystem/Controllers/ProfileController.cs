using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
