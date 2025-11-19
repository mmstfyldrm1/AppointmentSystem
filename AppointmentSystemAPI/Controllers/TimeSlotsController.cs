using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotsController : ControllerBase
    {
       private readonly ITimeSlotService _timeSlotService;

        public TimeSlotsController(ITimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var timeSlots = await _timeSlotService.GetList();
            return Ok(timeSlots);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _timeSlotService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dt_TimeSlots timeSlot)
        {
            _timeSlotService.Add(timeSlot);
            return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_TimeSlots timeSlot)
        {
            _timeSlotService.Update(timeSlot);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _timeSlotService.GetById(id);
            if (user == null)
                return NotFound();

            await _timeSlotService.Delete(user);
            return Ok();
        }
    }
}
