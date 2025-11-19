using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.ViewComponents.Default
{
    public class _AppointmentSystemSliderComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
