using AppointmentSystem.Services;
using AutoMapper;
using DTOLayer.GenericComboBoxListDtos;
using DTOLayer.ServicesDtos;
using DTOLayer.ShopDtos.AddShopOwnersDtos;
using DTOLayer.ShopDtos.ShopQueryDtos;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace AppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="ADMIN,SHOPOWNERS")]
    public class ServicesController : Controller
    {
        private readonly ApiClientService _apiClientService;
        private readonly IMapper _mapper;

        public ServicesController(ApiClientService apiClientService, IMapper mapper)
        {
            _apiClientService = apiClientService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async  Task<IActionResult> AddServices()
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
        public async Task<IActionResult> AddServices(AdminPanelAddServicesDto servicesDto,int ShopId)
        {


            var client = _apiClientService.CreateClient();
            servicesDto.ShopId = ShopId;    

            var ServiceDto = _mapper.Map<AdminPanelAddServicesDto>(servicesDto);
            var content1 = new StringContent(JsonConvert.SerializeObject(ServiceDto), Encoding.UTF8, "application/json");
            var response1 = await client.PostAsync("https://localhost:7179/api/Service", content1);
            if (response1.IsSuccessStatusCode)
            {

                return Redirect("~/Admin/Dashboard/Index");

            }


            var errorJson = await response1.Content.ReadAsStringAsync();
            Console.WriteLine(errorJson);
            return View();
          
        }


        [HttpGet]
        [Authorize(Roles = "ADMIN,SHOPOWNERS,WORKER")]
        public async Task<IActionResult> ServicesList()
        {
            var client = _apiClientService.CreateClient();
            int userId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
            if (userId == 0) { return RedirectToAction("Index", "Dashboard"); }
            int Id = 0;
            int.TryParse(User.FindFirst("ResultId").Value.ToString(), out Id);
            var sb = new StringBuilder();
            sb.AppendLine($"select ");
            sb.AppendLine($"isnull(se.ServiceName,'') [ServicesName]");
            sb.AppendLine($",isnull(se.ServicesType,'') [ServicesType]");
            sb.AppendLine($",isnull(se.Explanation,'') [Explanation]");
            sb.AppendLine($",isnull(se.Price,0) [Price]");
            sb.AppendLine($",ISNULL(s.Name,'') [ShopName]");
            sb.AppendLine($"");
            sb.AppendLine($"");
            sb.AppendLine($"from Dt_Services se");
            sb.AppendLine($"left join Dt_Shops s with(nolock) on s.Id=se.ShopId");
            sb.AppendLine($"left join Dt_ShopOwners so with(nolock) on so.Id=s.ShopOwnerId");
            sb.AppendLine($"where so.Id ={Id}");
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
            var values2 = JsonConvert.DeserializeObject<List<AdminPanelServicesListDto>>(jsonData2);
            return View(values2);
        }

    }
}
