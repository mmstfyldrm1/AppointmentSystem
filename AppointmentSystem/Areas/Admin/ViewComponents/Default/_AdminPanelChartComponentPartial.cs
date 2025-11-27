using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Areas.Admin.ViewComponents.Default
{
    public class _AdminPanelChartComponentPartial:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
