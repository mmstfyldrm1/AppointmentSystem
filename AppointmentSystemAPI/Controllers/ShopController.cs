using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {

        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _shopService.GetList();
            return Ok(user);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _shopService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dt_Shop shop)
        {
            _shopService.Add(shop);
            return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_Shop shop)
        {
            _shopService.Update(shop);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _shopService.GetById(id);
            if (user == null)
                return NotFound();

            await _shopService.Delete(user);
            return Ok();
        }
    }
}
