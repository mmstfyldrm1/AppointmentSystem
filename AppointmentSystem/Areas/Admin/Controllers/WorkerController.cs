using AppointmentSystem.Services;
using AutoMapper;
using DTOLayer.GenericComboBoxListDtos;
using DTOLayer.ResponseDtos;
using DTOLayer.ServicesDtos;
using DTOLayer.ShopDtos.ShopQueryDtos;
using DTOLayer.WorkerDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace AppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SHOPOWNERS,WORKER")]
    public class WorkerController : Controller
    {
        private readonly ApiClientService _apiClientService;
        private readonly IMapper _mapper;

        public WorkerController(ApiClientService apiClientService, IMapper mapper)
        {
            _apiClientService = apiClientService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddWorker()
        {
            Dictionary<int, string> ShopList = new Dictionary<int, string>();
            var Id = User.FindFirst("ResultId").Value.ToString();
            var client = _apiClientService.CreateClient();
            var sb = new StringBuilder();
            sb.AppendLine($"Select Id , Name from Dt_Shops where ShopOwnerId = {Id}");
            var queyobj = new { query = sb.ToString() };

            var content = new StringContent(JsonConvert.SerializeObject(queyobj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode) { }

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResponseComboBoxDto>>(jsonData);
            foreach (var item in values)
            {
                ShopList.Add(item.Id, item.Name.ToString());
            }

            ViewBag.Shops = ShopList.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWorker(CreateWorkerDto createWorkerDto)
        {
            var client = _apiClientService.CreateClient();
            int ApplicationUserId = 0;
            var sb = new StringBuilder();
            sb.AppendLine($"Select Id from AspNetUsers where Email = '{createWorkerDto.Email}'");
            var queyobj = new { query = sb.ToString() };

            var content = new StringContent(JsonConvert.SerializeObject(queyobj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode) { }

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResponseDto>>(jsonData);
            foreach (var item in values)
            {
                ApplicationUserId = item.Id;
            }
            createWorkerDto.ApplicationUserId = ApplicationUserId;
            var WorkerDto = _mapper.Map<CreateWorkerDto>(createWorkerDto);
            var content1 = new StringContent(JsonConvert.SerializeObject(WorkerDto), Encoding.UTF8, "application/json");
            var response1 = await client.PostAsync("https://localhost:7179/api/Worker", content1);
            if (response1.IsSuccessStatusCode)
            {

                return Redirect("~/Admin/Dashboard/Index");

            }


            var errorJson = await response1.Content.ReadAsStringAsync();
            Console.WriteLine(errorJson);
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

            sb.AppendLine($"SELECT ");
            sb.AppendLine($"    ISNULL(w.Name,'') AS WorkerName,");
            sb.AppendLine($"    ISNULL(s.Name,'') AS ShopName,");
            sb.AppendLine($"    ISNULL(w.WorkerPhone,'') AS WorkerPhone,");
            sb.AppendLine($"    ISNULL(w.WorkerImg,'') AS WorkerImg,");
            sb.AppendLine($"    CASE ");
            sb.AppendLine($"        WHEN COALESCE(w.WorkerStatus,1)=1 THEN 'Çalışıyor' ");
            sb.AppendLine($"        WHEN COALESCE(w.WorkerStatus,1)=0 THEN 'İzinli' ");
            sb.AppendLine($"        ELSE 'Bilinmiyor' ");
            sb.AppendLine($"    END AS WorkerStatus,");
            sb.AppendLine($"    ISNULL(ap.Randevu,0) AS TotalAppointmentCount,");
            sb.AppendLine($"    ISNULL(ap2.DayRandevu,0) AS TodayTotalAppointmentCount,");
            sb.AppendLine($"    ISNULL(w.InsertedDate,'') AS InsertedDate");
            sb.AppendLine($"FROM Dt_Workers w");
            sb.AppendLine($"LEFT JOIN Dt_Shops s WITH(NOLOCK) ON s.Id = w.ShopId");
            sb.AppendLine($"INNER JOIN Dt_ShopOwners so WITH(NOLOCK) ON so.Id = s.ShopOwnerId AND so.Id = {Id}");
            sb.AppendLine($"OUTER APPLY (");
            sb.AppendLine($"    SELECT COUNT(*) AS Randevu ");
            sb.AppendLine($"    FROM Dt_Appointments ap ");
            sb.AppendLine($"    WHERE ap.ShopId = s.Id");
            sb.AppendLine($") ap");
            sb.AppendLine($"OUTER APPLY (");
            sb.AppendLine($"    SELECT COUNT(*) AS DayRandevu ");
            sb.AppendLine($"    FROM Dt_Appointments ap ");
            sb.AppendLine($"    WHERE ap.ShopId = s.Id AND CAST(ap.AppointmentDate AS DATE) = '{DateTime.Now:yyyy-MM-dd}'");
            sb.AppendLine($") ap2");
            sb.AppendLine($"");
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


        [HttpGet]
        public async Task<IActionResult> UpdateWorker()
        {

            return View();
        }


        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> UpdateWorker(UpdateWorkerDto updateWorkerDto)
        {
            if (updateWorkerDto == null) 
            {
                return Redirect("~/Admin/Dashboard/Index");
            }
            var client = _apiClientService.CreateClient();
            int ApplicationUserId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out ApplicationUserId);
            int Id = 0;
            int.TryParse(User.FindFirst("ResultId").Value.ToString(), out Id);
            string email = User.FindFirst(ClaimTypes.Name)?.Value;
            updateWorkerDto.ApplicationUserId = ApplicationUserId;
            updateWorkerDto.Id =Id;
            updateWorkerDto.Email =email;

           
            var sbWor = new StringBuilder();
            sbWor.AppendLine($"Select ShopId  from Dt_Workers where Id = {Id}");
            var queyobjWor = new { query = sbWor.ToString() };

            var contentWor = new StringContent(JsonConvert.SerializeObject(queyobjWor), Encoding.UTF8, "application/json");
            var responseWor = await client.PostAsync("https://localhost:7179/api/Query/execute", contentWor);
            if (!responseWor.IsSuccessStatusCode) { }

            int ShopId = 0;
            var jsonDataWor = await responseWor.Content.ReadAsStringAsync();
            var valuesWor = JsonConvert.DeserializeObject<List<ResponseDto>>(jsonDataWor);

            foreach (var item in valuesWor)
            {
                 ShopId = item.Id;
            }
            updateWorkerDto.ShopId = ShopId;
            var WorkerDto = _mapper.Map<UpdateWorkerDto>(updateWorkerDto);
            var content1 = new StringContent(JsonConvert.SerializeObject(WorkerDto), Encoding.UTF8, "application/json");
            var response1 = await client.PutAsync("https://localhost:7179/api/Worker", content1);
            if (response1.IsSuccessStatusCode)
            {
                return Redirect("~/Admin/Dashboard/Index");
            }


            var errorJson = await response1.Content.ReadAsStringAsync();
            Console.WriteLine(errorJson);
            return View();


           
        }
    }
}
