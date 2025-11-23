using AppointmentSystem.Services;
using AutoMapper;
using DTOLayer.AppointmentDtos;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AppointmentSystem.Controllers
{
    [Authorize]
    public class AppointmentSystemController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ApiClientService _apiClientService;


        public AppointmentSystemController(IHttpContextAccessor httpContextAccessor, IMapper mapper, ApiClientService apiClientService)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _apiClientService = apiClientService;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddAppointment()
        {
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> AddAppointment(CreateAppointmentDto createAppointmentDtos)
        {

            var client = _apiClientService.CreateClient();

            int userId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
            createAppointmentDtos.ApplicationUserId= userId;    
            var  appointment = _mapper.Map<CreateAppointmentDto>(createAppointmentDtos);
            var content = new StringContent(JsonConvert.SerializeObject(appointment), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Appointment", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Randevu Oluşturuldu";
                return RedirectToAction("Index");
            }
                

            var errorJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(errorJson);

            return View(createAppointmentDtos);



           
        }

        [HttpGet]
       
        public async Task<IActionResult> AppointmentList(int page)
        {
            const int pageSize = 10;

            var sessionData = HttpContext.Session.GetString("searchResult");
            List<ResultSearchAppointmentDto> allAppointments = new List<ResultSearchAppointmentDto>();

            if (!string.IsNullOrEmpty(sessionData))
            {
                allAppointments = JsonConvert.DeserializeObject<List<ResultSearchAppointmentDto>>(sessionData);
            }
            int totalItems = allAppointments.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // sayfalama
            var appointments = allAppointments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // View'a sayfa bilgilerini de gönder
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(appointments);
        }



        [HttpPost]
        public async Task<IActionResult> PartialSearch(AppointmentSearchDto appointmentSearchDto)
        {

            var client = _apiClientService.CreateClient();
            var sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine("    Isnull(ts.Id,'') AS [TimeSlotId],");
            sb.AppendLine("    Isnull(s.Id,'') AS [ShopId],");
            sb.AppendLine("    Isnull(w.Id,'') AS [WorkerId],");
            sb.AppendLine("    Isnull(ts.Slot,'') AS [Time],");
            sb.AppendLine("    Isnull(w.Name,'') AS [WorkerName],");
            sb.AppendLine("    ISNULL(w.WorkerImg, '') AS [WorkerImg],");
            sb.AppendLine("    ISNULL(s.Name,'') AS [Name],");
            sb.AppendLine("    ISNULL(w.WorkerPhone,'') AS [WorkerPhone],");
            sb.AppendLine("    ISNULL(s.ShopPhone,'') AS [ShopPhone]");
            sb.AppendLine("FROM Dt_Workers w");
            sb.AppendLine($"JOIN Dt_Shops s WITH(NOLOCK) ON s.Id = w.ShopId AND s.ShopAddress = '{appointmentSearchDto.City}'");
            sb.AppendLine("CROSS JOIN Dt_TimeSlots ts");
            sb.AppendLine($"LEFT JOIN Dt_Appointments a ON a.WorkerId = w.Id AND a.AppointmentDate = '{appointmentSearchDto.Date:yyyy-MM-dd}' AND a.TimeSlotId = ts.Id");
            sb.AppendLine("WHERE a.Id IS NULL");
            sb.AppendLine("ORDER BY w.Id, ts.Id --FOR JSON PATH");
            var queryObj = new
            {
                query = sb.ToString()
            };

            var content = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "AppointmentSystem");


            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultSearchAppointmentDto>>(jsonData);
            //TempData["searchResult"] = JsonConvert.SerializeObject(values);
            HttpContext.Session.SetString("searchResult", JsonConvert.SerializeObject(values));

            return RedirectToAction("AppointmentList", "AppointmentSystem", values);

        }

        public async Task<IActionResult> MyActiveAppointment()
        {
            var client = _apiClientService.CreateClient();
            int userId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
            var sb = new StringBuilder();

            sb.AppendLine($"select      ");
            sb.AppendLine($"Isnull(ts.Slot,'') AS [Time],");
            sb.AppendLine($"Isnull(w.Name,'') AS [WorkerName],");
            sb.AppendLine($"ISNULL(w.WorkerImg, '') AS [WorkerImg],");
            sb.AppendLine($"ISNULL(s.Name,'') AS [Name],");
            sb.AppendLine($"ISNULL(w.WorkerPhone,'') AS [WorkerPhone],");
            sb.AppendLine($"ISNULL(s.ShopPhone,'') AS [ShopPhone]");
            sb.AppendLine($"from Dt_Appointments ap");
            sb.AppendLine($"left join Dt_Shops s with(nolock) on s.Id =ap.ShopId");
            sb.AppendLine($"left join Dt_Workers w with(nolock) on w.Id = ap.WorkerId");
            sb.AppendLine($"left join Dt_TimeSlots ts with(nolock) on ts.Id=ap.TimeSlotId");
            sb.AppendLine($"where AppointmentStatus = 1 and ApplicationUserId="+ userId.ToString());
            var queryObj = new
            {
                query = sb.ToString()
            };

            var content = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "AppointmentSystem");


            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<MyActiveAppointmentDto>>(jsonData);
            //TempData["searchResult"] = JsonConvert.SerializeObject(values);
            HttpContext.Session.SetString("searchResult", JsonConvert.SerializeObject(values));

            return View(values);

        }



    }
}

