using AppointmentSystem.Services;
using DTOLayer.AppointmentDtos;
using DTOLayer.ShopDtos.ShopQueryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppointmentSystem.ViewComponents.Default
{
    public class _AppointmentSystemStaticsComponentPartial:ViewComponent
    {
        private readonly ApiClientService _apiClientService;

        public _AppointmentSystemStaticsComponentPartial(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _apiClientService.CreateClient();
            var queryObj = new { query = "SELECT  (SELECT COUNT(*) FROM Dt_Shops) AS Shop, (SELECT COUNT(*) FROM Dt_Workers) AS Worker, (SELECT COUNT(*) FROM AspNetUsers) AS Users, (SELECT COUNT(*) FROM Dt_Appointments) AS Appointment;" };
            var content = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode)
                return View(new List<AppointmentStatictisDto>());

            var jsonData = await response.Content.ReadAsStringAsync();

            var values = JsonConvert.DeserializeObject<List<AppointmentStatictisDto>>(jsonData);
            return View(values);





        }
    }
}
