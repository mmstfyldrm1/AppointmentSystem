using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
      private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _discountService.GetList();
            return Ok(user);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _discountService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dt_Discount discount)
        {
            _discountService.Add(discount);
            return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_Discount discount)
        {
            _discountService.Update(discount);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _discountService.GetById(id);
            if (user == null)
                return NotFound();

            await _discountService.Delete(user);
            return Ok();
        }
    }
}
