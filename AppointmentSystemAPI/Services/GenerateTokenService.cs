using EntityLayer.Concrete;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppointmentSystemAPI.Services
{
    public class GenerateTokenService
    {
        private readonly IConfiguration _configuration;

        public GenerateTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(Dt_ApplicationUser user)
        {
            // 🔹 Token'a eklenecek claimler
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? user.UserName ?? user.Email)
            };

            // 🔹 Kullanıcının rolünü token'a ekle
            if (!string.IsNullOrEmpty(user.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
            }
            else
            {
                // Varsayılan rol veya kullanıcı tipine göre rol ata
                claims.Add(new Claim(ClaimTypes.Role, "MEMBER")); // Varsayılan
            }

            // 🔹 Key & Credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 🔹 Token nesnesi
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
