using AppointmentSystem.Services;
using DTOLayer.AppointmentDtos;
using DTOLayer.ShopDtos.ShopQueryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace AppointmentSystem.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ApiClientService _clientService;

        public PropertyController(ApiClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> PropertyListWithSearch(string searchKeyValue, int propertyId, string city)
        {

            var client = _clientService.CreateClient(); 
            searchKeyValue = TempData["searchKeyValue"].ToString();
            propertyId = int.Parse(TempData["propertyId"].ToString());
            city = TempData["City"].ToString();
            var queryObj = new { query = string.Format("select w.Name [WorkerName] ,s.Name [Name] ,t.Slot [Time] ,ap.AppointmentDate [AppointmentDate] from Dt_Appointments ap left join Dt_Workers w with(nolock) on w.Id=ap.WorkerId left join Dt_Shops s with(nolock) on s.Id=ap.ShopId left join Dt_TimeSlots t with(nolock) on t.Id=ap.TimeSlotId where ap.AppointmentDate='' and s.ShopAddress=''"), searchKeyValue, city };
            var content = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode)
                return View(new List<ResultSearchAppointmentDto>());

            var jsonData = await response.Content.ReadAsStringAsync();

            var values = JsonConvert.DeserializeObject<List<ResultSearchAppointmentDto>>(jsonData);
            return View(values);

            

        }
        /*
        [HttpGet]
        public async Task<IActionResult> PropertySingle(int Id)
        {
            ViewBag.i = Id;
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:44387/api/Products/GetProductByProductId?Id=" + Id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<ResultProductDto>(jsonData);

            var client2 = _httpClientFactory.CreateClient();
            var responseMessage2 = await client2.GetAsync("https://localhost:44387/api/Products/GetProductDetailByProductDetailId?Id=" + Id);
            var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<GetProductDetailByProductDetailIdDto>(jsonData2);
            int difDate = DateTime.Now.Month - values2.AdvertisementDate.Month;

            ViewBag.ID = values.productId;
            ViewBag.title1 = values.productTitle;
            ViewBag.Price = values.productPrice;
            ViewBag.City = values.productCity;
            ViewBag.District = values.productDistrict;
            ViewBag.Category = values.categoryName;
            ViewBag.Adress = values.ProductAdress;
            ViewBag.Img = values.ProductCoverImage;
            ViewBag.Type = values.ProductType;
            ViewBag.Description = values.ProductDescription;

            ViewBag.AdvDate = values2.AdvertisementDate.ToString("dd-MMM-yyyy");
            ViewBag.Location = values2.ProductDetailLocation;
            ViewBag.VideoUrl = values2.ProductDetailVideoUrl;
            ViewBag.DifDate = difDate;
            ViewBag.BuildYear = values2.ProductDetailBuildYear;
            ViewBag.BathCount = values2.ProductDetailBathCount;
            ViewBag.RoomCount = values2.ProductDetailBedRoomCount;
            ViewBag.Size = values2.ProductDetailSize;
            ViewBag.GarageSize = values2.ProductDetailGarageSize;

            return View();
        }
        */
    }
}
