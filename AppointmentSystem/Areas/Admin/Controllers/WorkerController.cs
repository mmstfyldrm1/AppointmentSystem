using DTOLayer.ShopDtos.ShopQueryDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SHOPOWNERS,WORKER")]
    public class WorkerController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> WorkerList()
        {
            var client = _apiClientService.CreateClient();
            int userId = 0;
            int ShopOwnerId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
            if (userId == 0) { return RedirectToAction("Index", "Dashboard"); }
            var sb1 = new StringBuilder();
            sb1.AppendLine($"select top 1  Id from Dt_ShopOwners where ApplicationUserId=" + userId.ToString());
            var queryObj = new
            {
                query = sb1.ToString()
            };

            var content = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResponseDto>>(jsonData);
            foreach (var item in values)
            {
                ShopOwnerId = item.Id;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"select ");
            sb.AppendLine($"ISNULL(s.Name,'') Name");
            sb.AppendLine($",ISNULL(ShopPhone,'') ShopPhone");
            sb.AppendLine($",ISNULL(ShopAddress,'') ShopAddress");
            sb.AppendLine($",ISNULL(Status,1) Status");
            sb.AppendLine($",isnull(ap.Randevu,0) TotalAppointmentCount");
            sb.AppendLine($",isnull(ap2.DayRandevu,0) TodayTotalAppointmentCount");
            sb.AppendLine($",isnull(w.Worker,'') WorkerCount");
            sb.AppendLine($"from Dt_Shops s");
            sb.AppendLine($"left join Dt_ShopOwners so with(nolock) on so.Id=s.ShopOwnerId");
            sb.AppendLine($"outer apply (select COUNT(*) [Randevu] from  Dt_Appointments ap where s.Id=ap.ShopId) ap");
            sb.AppendLine($"outer apply (select COUNT(*) [DayRandevu] from  Dt_Appointments ap where s.Id=ap.ShopId and ap.AppointmentDate='{DateTime.Now:yyyy-MM-dd}') ap2");
            sb.AppendLine($"outer apply (select COUNT(*) [Worker] from  Dt_Workers w where s.Id=w.ShopId) w");
            sb.AppendLine($"where so.Id ={ShopOwnerId} ");
            var queryObj2 = new
            {
                query = sb.ToString()
            };

            var content2 = new StringContent(JsonConvert.SerializeObject(queryObj2), Encoding.UTF8, "application/json");
            var response2 = await client.PostAsync("https://localhost:7179/api/Query/execute", content2);
            if (!response2.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");


            var jsonData2 = await response2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<List<AdminPanelShopListDto>>(jsonData2);


            return View(values2);


           
        }
    }
}
