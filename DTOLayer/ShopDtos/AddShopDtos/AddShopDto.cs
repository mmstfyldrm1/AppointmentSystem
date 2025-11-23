using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.ShopDtos.AddShopOwnersDtos
{
    public class AddShopDto
    {
      
        public string ShopName { get; set; }

        public int ShopOwnerId { get; set; }

        public string? ShopImg { get; set; }

        public string ShopAddress { get; set; }

        public string ShopPhone { get; set; }
    }
}
