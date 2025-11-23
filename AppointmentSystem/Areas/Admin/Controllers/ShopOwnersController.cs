using AppointmentSystem.Services;
using AutoMapper;
using DTOLayer.AppointmentDtos;
using DTOLayer.ShopOwnersDtos.AddShopOwnerDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace AppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SHOPOWNERS")]

    public class ShopOwnersController : Controller
    {
        private readonly ApiClientService _apiClientService;
        private readonly IMapper _mapper;

        public ShopOwnersController(ApiClientService apiClientService, IMapper mapper)
        {
            _apiClientService = apiClientService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddShopOwners()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddShopOwners(AddShopOwnersDto dto)
        {

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
                dto.ShopOwnerImg = ImageName;
            }
            dto.ApplicationUserId = userId;
            dto.Name = User.FindFirst(ClaimTypes.Name)?.Value;
            var client = _apiClientService.CreateClient();
            var shopOwnersDto = _mapper.Map<AddShopOwnersDto>(dto);
            var content = new StringContent(JsonConvert.SerializeObject(shopOwnersDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/ShopOwner", content);
            if (response.IsSuccessStatusCode)
            {

                return View();
            }



            var errorJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(errorJson);
            return View();
        }
    }
}
