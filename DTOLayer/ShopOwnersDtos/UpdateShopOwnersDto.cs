using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.ShopOwnersDtos
{
    public class UpdateShopOwnersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? ShopOwnerImg { get; set; }

        public string Email { get; set; }

        public IFormFile Image { get; set; }
        public string? ShopOwnerPhone { get; set; }


        public int ApplicationUserId { get; set; } // FK
    }
}
