using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Areas.Admin.ViewComponents.Default
{
    public class _AdminPanelMessagesComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
