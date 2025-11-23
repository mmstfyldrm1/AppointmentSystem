using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentServiceController : ControllerBase
    {
        private readonly IAppointmentserviceService _appointmentService;

        public AppointmentServiceController(IAppointmentserviceService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _appointmentService.GetList();
            return Ok(user);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _appointmentService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dt_AppointmentService appointmentServices)
        {
            _appointmentService.Add(appointmentServices);
            return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_AppointmentService appointmentServices)
        {
            _appointmentService.Update(appointmentServices);
           
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _appointmentService.GetById(id);
            if (user == null)
                return NotFound();

            await _appointmentService.Delete(user);
            return Ok();
        }
    }
}
