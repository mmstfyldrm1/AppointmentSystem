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

namespace AppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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


          
            var sb = new StringBuilder();
            sb.AppendLine($"select top 1  Id from Dt_ShopOwners where ApplicationUserId=" + userId.ToString());
            var queryObj = new
            {
                query = sb.ToString()
            };

            var content = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Error");


            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject <List<ResponseDto>>(jsonData);
            foreach (var item in values)
            {
                dto.ShopOwnerId = item.Id;
            }
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

            return View();
        }
    }
}
