
using AppointmentSystemAPI.Dtos.AuthUserDtos;
using AppointmentSystemAPI.Services;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Dt_ApplicationUser> _userManager;
        private readonly SignInManager<Dt_ApplicationUser> _signInManager;
        private readonly GenerateTokenService _generateTokenService;

        public AuthController(UserManager<Dt_ApplicationUser> userManager, SignInManager<Dt_ApplicationUser> signInManager, GenerateTokenService generateTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _generateTokenService = generateTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDtos dto)
        {
            var user = new Dt_ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Kayıt başarılı!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDtos dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı");

            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);
            if (!result.Succeeded)
                return Unauthorized("Geçersiz giriş");

          
            var token = _generateTokenService.CreateToken(user);
            return Ok(new { 
                
                Token = token,
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName



            });
        }
    }
}
