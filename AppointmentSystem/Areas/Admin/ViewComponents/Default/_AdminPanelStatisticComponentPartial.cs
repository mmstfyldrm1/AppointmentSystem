using AppointmentSystem.Services;
using DTOLayer.DashboardDtos;
using DTOLayer.GenericComboBoxListDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace AppointmentSystem.Areas.Admin.ViewComponents.Default
{
    public class _AdminPanelStatisticComponentPartial : ViewComponent
    {
        private readonly ApiClientService _apiClientService;
        public _AdminPanelStatisticComponentPartial(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _apiClientService.CreateClient();
            var ResultId = ((ClaimsPrincipal)User).FindFirst("ResultId")?.Value;
            var sb = new StringBuilder();

            sb.AppendLine($"SELECT");
            sb.AppendLine($"    -- Toplam Çalışan");
            sb.AppendLine($"    (SELECT COUNT(*) ");
            sb.AppendLine($"     FROM Dt_Workers W ");
            sb.AppendLine($"     WHERE W.ShopId IN (SELECT Id FROM Dt_Shops WHERE ShopOwnerId = {ResultId})");
            sb.AppendLine($"    ) AS WorkerCount,");
            sb.AppendLine($"");
            sb.AppendLine($"    -- Toplam Servis");
            sb.AppendLine($"    (SELECT COUNT(*) ");
            sb.AppendLine($"     FROM Dt_Services SV ");
            sb.AppendLine($"     WHERE SV.ShopId IN (SELECT Id FROM Dt_Shops WHERE ShopOwnerId = {ResultId})");
            sb.AppendLine($"    ) AS ServiceCount,");
            sb.AppendLine($"");
            sb.AppendLine($"    -- Toplam Randevu");
            sb.AppendLine($"    (SELECT COUNT(*) ");
            sb.AppendLine($"     FROM Dt_Appointments A");
            sb.AppendLine($"     WHERE A.ShopId IN (SELECT Id FROM Dt_Shops WHERE ShopOwnerId = {ResultId})");
            sb.AppendLine($"    ) AS AppointmentCount,");
            sb.AppendLine($"");
            sb.AppendLine($"    -- Toplam Dükkan Sayısı");
            sb.AppendLine($"    (SELECT COUNT(*) ");
            sb.AppendLine($"     FROM Dt_Shops ");
            sb.AppendLine($"     WHERE ShopOwnerId = {ResultId}");
            sb.AppendLine($"    ) AS ShopCount,");
            sb.AppendLine($"");
            sb.AppendLine($"    -- İptal Edilen Randevu Sayısı");
            sb.AppendLine($"    (SELECT COUNT(*) ");
            sb.AppendLine($"     FROM Dt_Appointments A1");
            sb.AppendLine($"     WHERE A1.AppointmentStatus = 0");
            sb.AppendLine($"       AND A1.ShopId IN (SELECT Id FROM Dt_Shops WHERE ShopOwnerId = {ResultId})");
            sb.AppendLine($"    ) AS AppointmentCancelCount;");


            var queyobj = new { query = sb.ToString() };

            var content = new StringContent(JsonConvert.SerializeObject(queyobj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode) { }

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<AdminPanelDashboardStatisticDto>>(jsonData);
            foreach (var item in values)
            {
                ViewBag.WorkerCount = item.WorkerCount;
                ViewBag.ServiceCount = item.ServiceCount;
                ViewBag.AppointmentCount = item.AppointmentCount;
                ViewBag.ShopCount = item.ShopCount;
                ViewBag.AppointmentCancelCount = item.AppointmentCancelCount;   
            }

            return View(values);
        }
    }
}
