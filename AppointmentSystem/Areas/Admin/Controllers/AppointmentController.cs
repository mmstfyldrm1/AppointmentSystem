using AppointmentSystem.Services;
using DTOLayer.AppointmentDtos;
using DTOLayer.ShopDtos.ShopQueryDtos;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text;

namespace AppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController : Controller
    {
        private readonly ApiClientService _apiClientService;

        public AppointmentController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AppointmentList()
        {
            var client = _apiClientService.CreateClient();
            string updateDate = "";


            int userId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
            if (userId == 0) { return RedirectToAction("Index", "Dashboard"); }
            int Id = 0;
            int.TryParse(User.FindFirst("ResultId").Value.ToString(), out Id);
            var sb = new StringBuilder();
            
            sb.AppendLine($"select ");
            sb.AppendLine($"a.AppointmentDate [AppointmentDate]");
            sb.AppendLine($",asp.FullName [MemberName]");
            sb.AppendLine($",w.Name [WorkerName]");
            sb.AppendLine($",t.Slot [Time]");
            sb.AppendLine($",ISnull(ss.ServiceName,'Hizmet Belirtilmemiş') [ServiceName]");
            sb.AppendLine($",a.AppointmentStatus [AppointmentStatus]");
            sb.AppendLine($",a.IsClosed [IsClosed] ");
            sb.AppendLine($",ISNULL(a.InsertadDate, '1900-01-01') AS [InsertedDate]");
            sb.AppendLine($",ISNULL(a.UpdateDate, '1900-01-01') AS [UpdateDate]");
            sb.AppendLine($"from Dt_Appointments a");
            sb.AppendLine($"Left join Dt_AppointmentServices aps with(nolock) on a.Id=aps.AppointmentId");
            sb.AppendLine($"left join Dt_Services ss with(nolock) on ss.Id =aps.ServiceId");
            sb.AppendLine($"left join Dt_Shops s with(nolock) on s.Id=a.ShopId");
            sb.AppendLine($"left join AspNetUsers asp with(nolock) on asp.Id = a.ApplicationUserId");
            sb.AppendLine($"left join Dt_Workers w with(nolock) on w.Id = a.WorkerId");
            sb.AppendLine($"left join Dt_TimeSlots t with(nolock) on t.Id = a.TimeSlotId");
            sb.AppendLine($"left join Dt_AppointmentServices ass with(nolock) on a.Id = ass.AppointmentId");
            sb.AppendLine($"where w.Id={Id}");

            var queryObj2 = new
            {
                query = sb.ToString()
            };

            var content2 = new StringContent(JsonConvert.SerializeObject(queryObj2), Encoding.UTF8, "application/json");
            var response2 = await client.PostAsync("https://localhost:7179/api/Query/execute", content2);
            if (!response2.IsSuccessStatusCode)
            {
                var errorJson = await response2.Content.ReadAsStringAsync();
                Console.WriteLine(errorJson);
                return Redirect("~Admin/Dashboard/Index");
            }


            var jsonData2 = await response2.Content.ReadAsStringAsync();
            if (jsonData2.Length == 0) { }
            var values2 = JsonConvert.DeserializeObject<List<GetAllAppointmentDto>>(jsonData2);
            return View(values2);
        }
    }
}
