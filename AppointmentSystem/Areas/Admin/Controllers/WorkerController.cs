using AppointmentSystem.Services;
using DTOLayer.ResponseDtos;
using DTOLayer.ShopDtos.ShopQueryDtos;
using DTOLayer.WorkerDtos;
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
        private readonly ApiClientService _apiClientService;

        public WorkerController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> WorkerList()
        {
            var client = _apiClientService.CreateClient();
            int userId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
            if (userId == 0) { return RedirectToAction("Index", "Dashboard"); }
            
            int Id = 0;
            int.TryParse(User.FindFirst("ResultId").Value.ToString(), out Id);

            var sb = new StringBuilder();

            sb.AppendLine($"select ");
            sb.AppendLine($"ISNULL(w.Name,'') WorkerName");
            sb.AppendLine($",ISNULL(s.Name,'') ShopName");
            sb.AppendLine($",ISNULL(w.WorkerPhone,'') WorkerPhone");
            sb.AppendLine($",ISNULL(w.WorkerImg,'') WorkerImg");
            sb.AppendLine($",Case when ISNULL(w.WorkerStatus,1)=1 then 'Çalışıyor' when ISNULL(w.WorkerStatus,1)= 0 then 'İzinli' else 'Bilinmiyor' end as WorkerStatus");
            sb.AppendLine($",isnull(ap.Randevu,0) TotalAppointmentCount");
            sb.AppendLine($",isnull(ap2.DayRandevu,0) TodayTotalAppointmentCount");
            sb.AppendLine($",isnull(w.InsertedDate,'') InsertedDate");
            sb.AppendLine($"from Dt_Workers w");
            sb.AppendLine($"left join Dt_Shops s with(nolock) on s.Id=w.ShopId");
            sb.AppendLine($"left join Dt_ShopOwners so with(nolock) on so.Id=s.ShopOwnerId");
            sb.AppendLine($"outer apply (select COUNT(*) [Randevu] from  Dt_Appointments ap where s.Id=w.ShopId) ap");
            sb.AppendLine($"outer apply (select COUNT(*) [DayRandevu] from  Dt_Appointments ap where s.Id=w.ShopId and ap.AppointmentDate='{DateTime.Now:yyyy-MM-dd}') ap2");
            sb.AppendLine($"where so.Id ={Id} ");
            var queryObj2 = new
            {
                query = sb.ToString()
            };

            var content2 = new StringContent(JsonConvert.SerializeObject(queryObj2), Encoding.UTF8, "application/json");
            var response2 = await client.PostAsync("https://localhost:7179/api/Query/execute", content2);
            if (!response2.IsSuccessStatusCode)
                return RedirectToAction("Index", "Dashboard");


            var jsonData2 = await response2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<List<AdminPanelWorkerDto>>(jsonData2);
            return View(values2);


           
        }
    }
}
