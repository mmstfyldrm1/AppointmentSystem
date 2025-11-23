using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var workers = await _workerService.GetList();   
            return Ok(workers);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var worker = await _workerService.GetById(id);   
            if (worker == null)
                return NotFound();

            return Ok(worker);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dt_Worker worker)
        {
            _workerService.Add(worker);
            return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_Worker worker)
        {
            _workerService.Update(worker);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _workerService.GetById(id);
            if (user == null)
                return NotFound();

            await _workerService.Delete(user);
            return Ok();
        }
    }
}
