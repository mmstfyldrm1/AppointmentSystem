using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Areas.Admin.ViewComponents.Default
{
    public class _AdminPanelUpcomingAppointmentsComponentPartial:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
