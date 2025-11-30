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
using Microsoft.Extensions.Logging.Abstractions;
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
        public async Task<IActionResult> AddAppointment(string WorkerId, string TimeSlotId, string shopId, string time, string shopName, string workerName, string date)
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
            createAppointmentDtos.AppointmentStatus = 1; //şimdilik ilerde ödemeden sonra 1 yapılcak
            var appointment = _mapper.Map<CreateAppointmentDto>(createAppointmentDtos);
            var content = new StringContent(JsonConvert.SerializeObject(appointment), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Appointment", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorJson1 = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorJson1);

            }

            int AppointmentId = 0;
            var sb = new StringBuilder();
            if (createAppointmentDtos.ShopId == null || createAppointmentDtos.TimeSlotId == null || createAppointmentDtos.WorkerId == null)
            { TempData["Error"] = "Hata"; }
            sb.AppendLine($"select top 1 Id  from Dt_Appointments where ApplicationUserId ={User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value} and ShopId ={createAppointmentDtos.ShopId} and WorkerId = {createAppointmentDtos.WorkerId} order by Id Desc ");
            var queyobj = new { query = sb.ToString() };

            var content1 = new StringContent(JsonConvert.SerializeObject(queyobj), Encoding.UTF8, "application/json");
            var response1 = await client.PostAsync("https://localhost:7179/api/Query/execute", content1);
            if (!response.IsSuccessStatusCode) { }

            var jsonData1 = await response1.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<List<ResponseDto>>(jsonData1);
            if (values2 != null)
            {
                foreach (var item in values2)
                {
                    AppointmentId = item.Id;
                }
            }
            CreateAppointmentServicesDto createAppointmentServicesDto = new CreateAppointmentServicesDto();
            createAppointmentServicesDto.AppointmentId = AppointmentId;
            createAppointmentServicesDto.ServiceId = createAppointmentDtos.ServicesId;


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
        public async Task<IActionResult> AppointmentList(string? Worker, string? Shop)
        {
            var sessionData = HttpContext.Session.GetString("searchResult");
            List<ResultSearchAppointmentDto> allAppointments = new List<ResultSearchAppointmentDto>();

            if (!string.IsNullOrEmpty(sessionData))
            {
                allAppointments = JsonConvert.DeserializeObject<List<ResultSearchAppointmentDto>>(sessionData);
            }

            // Filtreleme
            var FilterList = allAppointments
                .Where(a => (string.IsNullOrEmpty(Worker) || a.WorkerName.Contains(Worker, StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrEmpty(Shop) || a.ShopName.Contains(Shop, StringComparison.OrdinalIgnoreCase)))
                .ToList();


            ViewBag.WorkerFilter = Worker;
            ViewBag.ShopFilter = Shop;
            return View(FilterList);


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
            sb.AppendLine($"ts.Id [TimeSlotId],");
            sb.AppendLine($"w.Id [WorkerId],");
            sb.AppendLine($"s.Id [ShopId],");
            sb.AppendLine($"Isnull(ap.AppointmentDate,'1900-01-01') AS [AppointmentDate],");
            sb.AppendLine($"Isnull(ts.Slot,'') AS [Time],");
            sb.AppendLine($"Isnull(w.Name,'') AS [WorkerName],");
            sb.AppendLine($"ISNULL(w.WorkerImg, '') AS [WorkerImg],");
            sb.AppendLine($"ISNULL(s.Name,'') AS [ShopName],");
            sb.AppendLine($"ISNULL(w.WorkerPhone,'') AS [WorkerPhone],");
            sb.AppendLine($"ISNULL(s.ShopPhone,'') AS [ShopPhone]");
            sb.AppendLine($"from Dt_Appointments ap");
            sb.AppendLine($"left join Dt_Shops s with(nolock) on s.Id =ap.ShopId");
            sb.AppendLine($"left join Dt_Workers w with(nolock) on w.Id = ap.WorkerId");
            sb.AppendLine($"left join Dt_TimeSlots ts with(nolock) on ts.Id=ap.TimeSlotId");
            sb.AppendLine($"where ap.AppointmentStatus = 1 and ap.ApplicationUserId=" + userId.ToString());
            var queryObj = new
            {
                query = sb.ToString()
            };

            var content = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode)
            {
                var jsonDataErr = await response.Content.ReadAsStringAsync();

                return RedirectToAction("Index", "AppointmentSystem");


            }


            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<MyActiveAppointmentDto>>(jsonData);
            //TempData["searchResult"] = JsonConvert.SerializeObject(values);
            HttpContext.Session.SetString("searchResult1", JsonConvert.SerializeObject(values));

            return View(values);

        }


        [HttpGet]
        public async Task<IActionResult> UpdateAppointment(string WorkerId, string TimeSlotId, string shopId, string ShopName, string AppointmentDate)
        {
            var client = _apiClientService.CreateClient();

            Dictionary<int, string> ServicesList = new Dictionary<int, string>();
            int Id = 0;
            if (DateTime.TryParse(AppointmentDate.ToString(), out DateTime parsed))
            {
                ViewBag.Date = parsed.ToString("yyyy-MM-dd"); // date input formatı
            }
            else
            {
                ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            }



            ViewBag.ShopName = ShopName;


            ViewBag.WorkerId = WorkerId;
            ViewBag.TimeSlotId = TimeSlotId;
            ViewBag.ShopId = shopId;



            var sb = new StringBuilder();
            sb.AppendLine($"Select Id , ServiceName [Name] from Dt_Services where ShopId = {shopId}");
            var queyobj = new { query = sb.ToString() };

            var content = new StringContent(JsonConvert.SerializeObject(queyobj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode) { }

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResponseComboBoxDto>>(jsonData);
            if (values != null)
            {
                foreach (var items in values)
                {
                    ServicesList.Add(items.Id, items.Name.ToString());
                }
            }

            ViewBag.Services = ServicesList.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }).ToList();

            /*--------------------------------------------------------------------------------------------------------------------*/

            Dictionary<int, string> WorkerList = new Dictionary<int, string>();

            var sbWor = new StringBuilder();
            sbWor.AppendLine($"Select Id , Name [Name] from Dt_Workers where ShopId = {shopId}");
            var queyobjWor = new { query = sbWor.ToString() };

            var contentWor = new StringContent(JsonConvert.SerializeObject(queyobjWor), Encoding.UTF8, "application/json");
            var responseWor = await client.PostAsync("https://localhost:7179/api/Query/execute", contentWor);
            if (!responseWor.IsSuccessStatusCode) { }

            var jsonDataWor = await responseWor.Content.ReadAsStringAsync();
            var valuesWor = JsonConvert.DeserializeObject<List<ResponseComboBoxDto>>(jsonDataWor);
            if (valuesWor != null)
            {
                foreach (var items in valuesWor)
                {
                    WorkerList.Add(items.Id, items.Name.ToString());
                }
            }

            ViewBag.Worker = WorkerList.Select(a => new SelectListItem
            {
                Value = a.Key.ToString(),
                Text = a.Value
            }).ToList();

            /*-------------------------------------------------------------------------------------------------------------*/

            Dictionary<int, string> TimeSlot = new Dictionary<int, string>();
            string dateStr = parsed.ToString("yyyy-MM-dd");
            var sbTime = new StringBuilder();

            sbTime.AppendLine($"SELECT ");
            sbTime.AppendLine($"    t.Id,");
            sbTime.AppendLine($"    t.Slot AS [Name]");
            sbTime.AppendLine($"FROM Dt_TimeSlots t");
            sbTime.AppendLine($"WHERE NOT EXISTS (");
            sbTime.AppendLine($"    SELECT 1");
            sbTime.AppendLine($"    FROM Dt_Appointments a");
            sbTime.AppendLine($"    WHERE a.TimeSlotId = t.Id");
            sbTime.AppendLine($"      AND a.ShopId = {shopId}");
            sbTime.AppendLine($"      AND a.AppointmentDate = '{dateStr}'");
            sbTime.AppendLine(")");
            var queyobjTime = new { query = sbTime.ToString() };

            var contentTime = new StringContent(JsonConvert.SerializeObject(queyobjTime), Encoding.UTF8, "application/json");
            var responseTime = await client.PostAsync("https://localhost:7179/api/Query/execute", contentTime);
            if (!responseTime.IsSuccessStatusCode) { }

            var jsonDataTime = await responseTime.Content.ReadAsStringAsync();
            var valuesTime = JsonConvert.DeserializeObject<List<ResponseComboBoxDto>>(jsonDataTime);
            if (valuesTime != null)
            {
                foreach (var items in valuesTime)
                {
                    TimeSlot.Add(items.Id, items.Name.ToString());
                }
            }

            ViewBag.TimeSlot = TimeSlot.Select(b => new SelectListItem
            {
                Value = b.Key.ToString(),
                Text = b.Value
            }).ToList();


            /*------------------------------------------------------------------------------------------------------------------*/





            return View();
        }


        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> UpdateAppointment(UpdateAppointmentDto updateAppointmentDto)
        {
            if (updateAppointmentDto.ShopId == null || updateAppointmentDto.TimeSlotId == null || updateAppointmentDto.WorkerId == null)
            {
                TempData["Error"] = "Gerekli alanlar eksik!";
                return View(updateAppointmentDto);
            }

            var client = _apiClientService.CreateClient();

            // Kullanıcı ID'sini al
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out int userId);
            updateAppointmentDto.ApplicationUserId = userId;
            updateAppointmentDto.AppointmentStatus = 1; // Şimdilik 1, ileride ödemeden sonra değişebilir
            
            // Son randevuyu al
            var sbAppointment = new StringBuilder();
            sbAppointment.AppendLine($@"
        SELECT TOP 1 Id 
        FROM Dt_Appointments 
        WHERE ApplicationUserId = {userId} 
          AND ShopId = {updateAppointmentDto.ShopId} 
          AND WorkerId = {updateAppointmentDto.WorkerId} 
          AND TimeSlotId={updateAppointmentDto.TimeSlotId}
        ORDER BY Id DESC
    ");
            var queryObj = new { query = sbAppointment.ToString() };
            var contentQuery = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
            var responseQuery = await client.PostAsync("https://localhost:7179/api/Query/execute", contentQuery);

            if (!responseQuery.IsSuccessStatusCode)
            {
                TempData["Error"] = "Randevu sorgulanamadı!";
                return View(updateAppointmentDto);
            }

            var jsonData = await responseQuery.Content.ReadAsStringAsync();
            var appointments = JsonConvert.DeserializeObject<List<ResponseDto>>(jsonData);
            if (appointments == null || appointments.Count == 0)
            {
                TempData["Error"] = "Randevu bulunamadı!";
                return View(updateAppointmentDto);
            }

            int appointmentId = appointments[0].Id;
            updateAppointmentDto.Id= appointmentId;

            // Randevuyu güncelle
            var appointmentContent = new StringContent(JsonConvert.SerializeObject(updateAppointmentDto), Encoding.UTF8, "application/json");
            var responseUpdate = await client.PutAsync($"https://localhost:7179/api/Appointment", appointmentContent);

            if (!responseUpdate.IsSuccessStatusCode)
            {
                var errorJson = await responseUpdate.Content.ReadAsStringAsync();
                Console.WriteLine(errorJson);
                TempData["Error"] = "Randevu güncellenemedi!";
                return View(updateAppointmentDto);
            }

            // Servis ID'sini al
            var sbService = new StringBuilder();
            sbService.AppendLine($@"
        SELECT TOP 1 Id 
        FROM Dt_AppointmentServices 
        WHERE AppointmentId = {appointmentId} 
          AND ServiceId = {updateAppointmentDto.ServicesId} 
        ORDER BY Id DESC
    ");
            var queryServiceObj = new { query = sbService.ToString() };
            var contentServiceQuery = new StringContent(JsonConvert.SerializeObject(queryServiceObj), Encoding.UTF8, "application/json");
            var responseServiceQuery = await client.PostAsync("https://localhost:7179/api/Query/execute", contentServiceQuery);

            if (!responseServiceQuery.IsSuccessStatusCode)
            {
                TempData["Error"] = "Randevu servisi sorgulanamadı!";
                return View(updateAppointmentDto);
            }
            int serviceId = 0;
            var jsonServiceData = await responseServiceQuery.Content.ReadAsStringAsync();
            var services = JsonConvert.DeserializeObject<List<ResponseDto>>(jsonServiceData);
            foreach (var item in services)
            {
                 serviceId = item.Id;
            }

            // Randevu servisini güncelle
            var updateServiceDto = new UpdateAppointmentServiceDto
            {
                Id = serviceId,
                AppointmentId = appointmentId,
                ServiceId = updateAppointmentDto.ServicesId
            };

            var serviceContent = new StringContent(JsonConvert.SerializeObject(updateServiceDto), Encoding.UTF8, "application/json");
            var responseServiceUpdate = await client.PutAsync($"https://localhost:7179/api/AppointmentService", serviceContent);

            if (!responseServiceUpdate.IsSuccessStatusCode)
            {
                var errorJson = await responseServiceUpdate.Content.ReadAsStringAsync();
                Console.WriteLine(errorJson);
                TempData["Error"] = "Randevu servisi güncellenemedi!";
                return View(updateAppointmentDto);
            }

            TempData["Success"] = "Randevu ve servis başarıyla güncellendi!";
            return RedirectToAction("Index");
        }





    }
}

