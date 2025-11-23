using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using DataAccsessLayer.Concrete.UoW;


namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
       private readonly IAppointmentService _appointmentService;

      

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result =await _appointmentService.GetList();
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
                return NotFound();

            return Ok(appointment);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dt_Appointment appointment)
        {
            appointment.AppointmentDate = appointment.AppointmentDate.Date;
            _appointmentService.Add(appointment);
            return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_Appointment appointment)
        {
            _appointmentService.Update(appointment);
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
