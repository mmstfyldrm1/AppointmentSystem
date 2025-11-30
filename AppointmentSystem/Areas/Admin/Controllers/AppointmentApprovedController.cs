using AppointmentSystem.Services;
using DTOLayer.AppointmentDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AppointmentApprovedController : Controller
    {
        private readonly ApiClientService _apiClientService;

        public AppointmentApprovedController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ApprovedAppointment()
        {
            string aliasTable = string.Empty;
            if (User.IsInRole("SHOPOWNERS") || User.IsInRole("ADMIN"))
            {
                aliasTable = "so";
            }

            if (User.IsInRole("WORKERS") || User.IsInRole("ADMIN"))
            {
                aliasTable = "wo";
            }
            int Id = 0;
            int.TryParse(User.FindFirst("ResultId").Value.ToString(), out Id);

            var client = _apiClientService.CreateClient();
            var sb = new StringBuilder();
            sb.AppendLine($"select ");
            sb.AppendLine($"s.Id [ShopId]");
            sb.AppendLine($",ap.Id [AppointmentId]");
            sb.AppendLine($",ISNULL(asp.FullName,'') [UserName]");
            sb.AppendLine($",ap.AppointmentDate [AppointmentDate]");
            sb.AppendLine($",case when ap.IsClosed=0 then 'Aktif' when ap.IsClosed=1 then 'Kapalı' else '' end as  [Randevu Durumu]");
            sb.AppendLine($",Isnull(s.Name,'') [ShopName]");
            sb.AppendLine($",Isnull(wo.Name,'') [WorkerName]");
            sb.AppendLine($",Isnull(t.Slot,'') [TimeSlot]");
            sb.AppendLine($",Isnull(se.ServiceName,'') [ServiceName]");
            sb.AppendLine($",ap.AppointmentStatus [AppointmentStatus]");
            sb.AppendLine($",Isnull(se.Price,0) [Price]");
            sb.AppendLine($"from Dt_Appointments ap");
            sb.AppendLine($"left join AspNetUsers asp with(nolock) on asp.Id = ap.ApplicationUserId");
            sb.AppendLine($"left join Dt_Workers wo with(nolock) on wo.Id =ap.WorkerId");
            sb.AppendLine($"left join Dt_Shops s with(nolock) on s.Id = ap.ShopId");
            sb.AppendLine($"left join Dt_ShopOwners so with(nolock) on so.Id = s.ShopOwnerId");
            sb.AppendLine($"left join Dt_AppointmentServices ass with(nolock) on ap.Id =ass.AppointmentId");
            sb.AppendLine($"left join Dt_Services se with(nolock) on se.Id =ass.ServiceId");
            sb.AppendLine($"left join Dt_TimeSlots t with(nolock) on t.Id = ap.TimeSlotId");
            sb.AppendLine($"where {aliasTable}.Id = {Id}  ");

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
            if (jsonData2.Length == 0)
            {

            }
            var values2 = JsonConvert.DeserializeObject<List<ApprovedAppointmentDto>>(jsonData2);
            return View(values2);

        }

        [HttpGet]
        [Route("ApproveAppointment/{id:int}")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            try
            {
                var client = _apiClientService.CreateClient();
                var sb = new StringBuilder();
                sb.AppendLine($"UPDATE Dt_Appointments SET AppointmentStatus = 1 WHERE Id = {id}");

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
                    TempData["Error"] = "Randevu onaylanırken hata oluştu!";
                    return Redirect("~/Admin/Dashboard/Index");
                }

                TempData["Success"] = "Randevu başarıyla onaylandı!";
                return Redirect("~/Admin/Dashboard/Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return Redirect("~/Admin/Dashboard/Index");
            }
        }


    }
}
