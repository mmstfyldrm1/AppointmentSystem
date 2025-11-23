using AppointmentSystem.Services;
using DTOLayer.ShopOwnersDtos.AddShopOwnerDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Xml.Linq;

namespace AppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AddShopOwnersController : Controller
    {
        private readonly ApiClientService _apiClientService;
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddShopOwners(AddShopOwnersDto dto)
        {

            //int userId = 0;
            //int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out userId);
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
            //dto.ApplicationUserId = userId;
           // dto.ShopOwnerName =  User.FindFirst(ClaimTypes.Name)?.Value;
            //ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value;


            return View();
        }
    }
}
