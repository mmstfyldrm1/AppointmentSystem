using DTOLayer.ResponseDtos;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AppointmentSystemAPI.Services
{
    public class GenerateTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Dt_ApplicationUser> _userManager;
        private readonly ApiClientService _apiClientService;
        public GenerateTokenService(IConfiguration configuration, UserManager<Dt_ApplicationUser> userManager, ApiClientService apiClientService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _apiClientService = apiClientService;
        }

        public async Task<string> CreateToken(Dt_ApplicationUser user)
        {

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FullName ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                var queryObj = new
                {
                    query = ""
                };
                int ResultId = 0;
                var client = _apiClientService.CreateClient();
                var sb = new StringBuilder();
                if (role == "SHOPOWNERS")
                {
                    sb.AppendLine($"select top 1  Id from Dt_ShopOwners where ApplicationUserId=" + user.Id.ToString());
                    queryObj = new
                    {
                        query = sb.ToString()
                    };
                }
                else if (role == "WORKER")
                {
                    sb.AppendLine($"select top 1  Id from Dt_Workers where ApplicationUserId=" + user.Id.ToString());
                    queryObj = new
                    {
                        query = sb.ToString()
                    };
                }

                if (role != "MEMBER")
                {
                    var content = new StringContent(JsonConvert.SerializeObject(queryObj), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://localhost:7179/api/Query/execute", content);
                    if (!response.IsSuccessStatusCode) { }



                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<ResponseDto>>(jsonData);
                    foreach (var item in values)
                    {
                        ResultId = item.Id;
                    }

                    claims.Add(new Claim(ClaimTypes.Role, role));
                    claims.Add(new Claim("ResultId", ResultId.ToString()));
                }
            }



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString.ToString();
        }
    }
}
