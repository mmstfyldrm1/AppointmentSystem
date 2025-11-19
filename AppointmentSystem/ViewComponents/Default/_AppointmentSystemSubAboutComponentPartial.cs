using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.ViewComponents.Default
{
    public class _AppointmentSystemSubAboutComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
