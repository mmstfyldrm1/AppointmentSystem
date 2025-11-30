using AppointmentSystem.Services;
using AutoMapper;
using DTOLayer.AppointmentDtos;
using DTOLayer.ResponseDtos;
using DTOLayer.ShopDtos.AddShopOwnersDtos;
using DTOLayer.ResponseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using DTOLayer.ShopDtos.ShopQueryDtos;
using Humanizer;

namespace AppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SHOPOWNERS")]
    public class ShopController : Controller
    {
        private readonly ApiClientService _apiClientService;
        private readonly IMapper _mapper;

        public ShopController(ApiClientService apiClientService, IMapper mapper)
        {
            _apiClientService = apiClientService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddShop()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddShop(AddShopDto dto)
        {
            var client = _apiClientService.CreateClient();
            int userId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
            if (dto.Image != null)
            {
                var resource = Directory.GetCurrentDirectory();
                var extension = Path.GetExtension(dto.Image.FileName);
                var ImageName = Guid.NewGuid() + extension;
                var saveLacotion = resource + "/wwwroot/userimages/" + ImageName;
                var stream = new FileStream(saveLacotion, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
                dto.ShopImg = ImageName;
            }


            int Id = 0;
            int.TryParse(User.FindFirst("ResultId").Value.ToString(), out Id);
            dto.ShopOwnerId = Id;
            var shopOwnersDto = _mapper.Map<AddShopDto>(dto);
            var content1 = new StringContent(JsonConvert.SerializeObject(shopOwnersDto), Encoding.UTF8, "application/json");
            var response1 = await client.PostAsync("https://localhost:7179/api/Shop", content1);
            if (response1.IsSuccessStatusCode)
            {

                return Redirect("~/Admin/Dashboard/Index");

            }


            var errorJson = await response1.Content.ReadAsStringAsync();
            Console.WriteLine(errorJson);
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> ShopList()
        {
            var client = _apiClientService.CreateClient();
            int userId = 0;
            int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
            if (userId == 0) { return RedirectToAction("Index", "Dashboard"); }
            int Id = 0;
            int.TryParse(User.FindFirst("ResultId").Value.ToString(), out Id);
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
            sb.AppendLine($"where so.Id ={Id} ");
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
            var values2 = JsonConvert.DeserializeObject<List<AdminPanelShopListDto>>(jsonData2);



            return View(values2);


        }
    }
}
