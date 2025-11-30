using AppointmentSystem.Services;
using DTOLayer.ShopDtos.ShopQueryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppointmentSystem.ViewComponents.Default
{
    public class _AppointmentSystemCompanyListComponentPartial:ViewComponent
    {
        private readonly ApiClientService _apiClientService;

        public _AppointmentSystemCompanyListComponentPartial(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _apiClientService.CreateClient(); 
            var queryObj = new { query = "select top 6 s.Name [ShopName],s.ShopImg,s.ShopAddress,s.ShopPhone, so.Name as Name  from Dt_Shops s with(nolock) left join Dt_ShopOwners so with(nolock) on so.Id=s.ShopOwnerId" };
            var content = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
            if (!response.IsSuccessStatusCode)
                return View(new List<AppointmentTop10ShopDto>());

            var jsonData = await response.Content.ReadAsStringAsync();

            var values = JsonConvert.DeserializeObject<List<AppointmentTop10ShopDto>>(jsonData);
            return View(values);





        }
    }
}
