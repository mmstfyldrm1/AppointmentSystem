using AppointmentSystem.Services;
using AutoMapper;
using DTOLayer.AppointmentDtos;
using DTOLayer.GenericComboBoxListDtos;
using DTOLayer.ResponseDtos;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> AddAppointment( string WorkerId, string TimeSlotId ,string shopId, string time, string shopName, string workerName, string date)
        {
            var client = _apiClientService.CreateClient();
            Dictionary<int, string> ServicesList = new Dictionary<int, string>();
            int Id = 0;
            if (DateTime.TryParse(date, out DateTime parsed))
            {
                ViewBag.Date = parsed.ToString("yyyy-MM-dd"); // date input formatı
            }
            else
            {
                ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            }


            ViewBag.WorkerName = workerName;
            ViewBag.ShopName = shopName;
            ViewBag.Time = time;

            ViewBag.WorkerId = WorkerId;
            ViewBag.TimeSlotId = TimeSlotId;
            ViewBag.ShopId = shopId;


            int.TryParse(shopId, out Id);
            var sb = new StringBuilder();
            sb.AppendLine($"Select Id , ServiceName [Name] from Dt_Services where ShopId = {Id}");
            var queyobj = new { query = sb.ToString() };

            var content = new StringContent(JsonConvert.SerializeObject(queyobj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode) { }

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResponseComboBoxDto>>(jsonData);
            if (values != null)
            {
                foreach (var item in values)
                {
                    ServicesList.Add(item.Id, item.Name.ToString());
                }
            }

            ViewBag.Services = ServicesList.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }).ToList();



            return View();
        }

        [HttpPost]

        public async Task<IActionResult> AddAppointment(CreateAppointmentDto createAppointmentDtos)
        {

            var client = _apiClientService.CreateClient();

            int userId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
            createAppointmentDtos.ApplicationUserId = userId;
            var appointment = _mapper.Map<CreateAppointmentDto>(createAppointmentDtos);
            var content = new StringContent(JsonConvert.SerializeObject(appointment), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Appointment", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Randevu Oluşturuldu";
              
            }

            int AppointmentId = 0;
            var sb = new StringBuilder();
            if(createAppointmentDtos.ShopId ==null || createAppointmentDtos.TimeSlotId == null || createAppointmentDtos.WorkerId ==null )
            { TempData["Error"] = "Hata"; }
            sb.AppendLine($"select Id  from Dt_Appointments where ApplicationUserId ={User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value} and ShopId ={createAppointmentDtos.ShopId} and WorkerId = {createAppointmentDtos.WorkerId} ");
            var queyobj = new { query = sb.ToString() };

            var content1 = new StringContent(JsonConvert.SerializeObject(queyobj), Encoding.UTF8, "application/json");
            var response1 = await client.PostAsync("https://localhost:7179/api/Query/execute", content1);
            if (!response.IsSuccessStatusCode) { }

            var jsonData1 = await response1.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<List<ResponseDto>>(jsonData1);

            foreach (var item in values2)
            {
                AppointmentId = item.Id;
            }

            CreateAppointmentServicesDto createAppointmentServicesDto= new CreateAppointmentServicesDto();
            createAppointmentServicesDto.AppointmentId = AppointmentId;
            createAppointmentServicesDto.ServicesId = createAppointmentDtos.ServicesId;


            var appointmentServices = _mapper.Map<CreateAppointmentServicesDto>(createAppointmentServicesDto);
            var contentSer = new StringContent(JsonConvert.SerializeObject(appointmentServices), Encoding.UTF8, "application/json");
            var responseSer = await client.PostAsync("https://localhost:7179/api/AppointmentService", contentSer);
            if (responseSer.IsSuccessStatusCode)
            {
                TempData["Success"] = "Servis Oluşturuldu";
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
            sb.AppendLine("    ISNULL(s.Name,'') AS [ShopName],");
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
            foreach (var item in values)
            {
                item.Date = appointmentSearchDto.Date;

            }
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
            sb.AppendLine($"where AppointmentStatus = 1 and ApplicationUserId=" + userId.ToString());
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

