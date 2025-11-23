using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOLayer.ShopDtos.AddShopOwnersDtos
{
    public class AddShopDto
    {
      
        public string Name { get; set; }

        public int ShopOwnerId { get; set; }

        public string? ShopImg { get; set; }

        [JsonIgnore]
        public IFormFile Image {  get; set; }   

        public string ShopAddress { get; set; }

        public string ShopPhone { get; set; }
    }
}
