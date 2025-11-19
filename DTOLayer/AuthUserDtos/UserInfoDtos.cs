using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.AuthUserDtos
{
    public class UserInfoDtos
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public DateTime ExpireDate { get; set; } = DateTime.UtcNow.AddHours(2);
    }
}
