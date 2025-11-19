using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppointmentSystemAPI.Dtos;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherUserController : ControllerBase
    {
        private  readonly IOtherUserService _userService;

       

        public OtherUserController(IOtherUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userService.GetList(); ;  
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public   IActionResult Add([FromBody] Dt_OtherUser user)
        {
            _userService.Add(user);
           
             return Ok("Ok");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Dt_OtherUser user)
        {
            _userService.Update(user);
            
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound();

            await _userService.Delete(user);
            return Ok();
        }
    }
}

