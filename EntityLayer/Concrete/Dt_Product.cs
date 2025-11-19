using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public int? ShopId { get; set; }

        public string? ProductImg { get; set; }

        [JsonIgnore]
        public  Dt_Shop? Shop { get; set; }

        public int? ProductCategoryId { get; set; }
       
        [JsonIgnore]
        public  Dt_ProductCategory? ProductCategory { get; set; }
        
        [JsonIgnore]
        public  ICollection<Dt_AppointmentProduct>? AppointmentProducts { get; set; }
    }
}
