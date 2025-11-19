using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ShopOwnerId { get; set; }

        public string? ShopImg { get; set; }

        public string ShopAddress { get; set; }

        public string ShopPhone { get; set; }

        [JsonIgnore]
        public  Dt_ShopOwner? ShopOwner { get; set; }
       
        [JsonIgnore]
        public  ICollection<Dt_Worker>? Workers { get; set; }
        
        [JsonIgnore]
        public  ICollection<Dt_Service>? Services { get; set; }
        
        [JsonIgnore]
        public  ICollection<Dt_Product>? Products { get; set; }
        
        [JsonIgnore]
        public  ICollection<Dt_Appointment>? Appointments { get; set; }
    }
}
