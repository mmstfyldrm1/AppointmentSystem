using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using DataAccsessLayer.Concrete.UoW;
using AppointmentSystemAPI.Services;


namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly NotificationService _notificationService;

        public AppointmentController(IAppointmentService appointmentService, NotificationService notificationService)
        {
            _appointmentService = appointmentService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _appointmentService.GetList();
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
        public async Task<IActionResult> Add([FromBody] Dt_Appointment appointment)
        {
            appointment.AppointmentDate = appointment.AppointmentDate.Date;
            _appointmentService.Add(appointment);

            await _notificationService.SendToWorker(appointment.WorkerId.ToString(),
                    $"Yeni randevu: {appointment.AppointmentDate} {appointment.TimeSlotId.ToString()}");

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
