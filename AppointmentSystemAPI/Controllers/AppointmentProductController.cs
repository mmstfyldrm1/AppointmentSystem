using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentProductController : ControllerBase
    {
        private readonly IAppointmentProductService _appointmentProductService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _appointmentProductService.GetList();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _appointmentProductService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dt_AppointmentProduct appointmentProduct)
        {
            _appointmentProductService.Add(appointmentProduct);
            
            return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_AppointmentProduct appointmentProduct)
        {
            _appointmentProductService.Update(appointmentProduct);
           
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _appointmentProductService.GetById(id);
            if (user == null)
                return NotFound();

            await _appointmentProductService.Delete(user);
            
            return Ok();
        }
    }
}
