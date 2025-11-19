using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopOwnerController : ControllerBase
    {
       private readonly IShopOwnerService _shopOwnerService;

        public ShopOwnerController(IShopOwnerService shopOwnerService)
        {
            _shopOwnerService = shopOwnerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _shopOwnerService.GetList();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _shopOwnerService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dt_ShopOwner owner)
        {
            _shopOwnerService.Add(owner);
            return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_ShopOwner owner)
        {
            _shopOwnerService.Update(owner);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _shopOwnerService.GetById(id);
            if (user == null)
                return NotFound();

            await _shopOwnerService.Delete(user);
            return Ok();
        }
    }
}
