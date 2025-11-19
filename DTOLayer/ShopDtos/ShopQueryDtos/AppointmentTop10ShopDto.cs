using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOLayer.ShopDtos.ShopQueryDtos
{
    public class AppointmentTop10ShopDto
    {
        public string Name { get; set; }

        public int ShopOwnerId { get; set; }

        public string? ShopImg { get; set; } = " ";

        public string ShopAddress { get; set; }

        public string ShopPhone { get; set; }

       
        public string ShopOwnerName { get; set; }
    }
}
