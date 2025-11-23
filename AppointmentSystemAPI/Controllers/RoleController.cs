using AppointmentSystemAPI.Dtos.AuthUserDtos;
using AppointmentSystemAPI.Dtos.RolesDtos;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Dt_ApplicationRole> _roleManager;
        private readonly UserManager<Dt_ApplicationUser> _UserManager;

        public RoleController(RoleManager<Dt_ApplicationRole> roleManager, UserManager<Dt_ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _UserManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateRole(CreateRoleDto dto)
        {
            var response = await _roleManager.CreateAsync(new Dt_ApplicationRole
            {
                Name = dto.Name,    
            });

            if (response.Succeeded)
            {
                return Ok("Kayıt başarılı!");
            }
            else
                return BadRequest(response.Errors);



        }

        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(RoleDto dto)
        {
           var userDetails= await _UserManager.FindByEmailAsync(dto.Email);

            if (userDetails != null)
            {
                var Response = await _UserManager.AddToRoleAsync(userDetails, dto.Name);  
                if (Response.Succeeded) 
                {
                    return Ok("Kayıt Başarılı");
                }

                return BadRequest(Response.Errors);
            }


            return BadRequest("Bilinmeyen bir hata");
           



        }

    }

}
