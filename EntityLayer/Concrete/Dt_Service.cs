using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_Service
    {

        public int Id { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }

        public int ShopId { get; set; }
       
        [JsonIgnore]
        public  Dt_Shop? Shop { get; set; }
        
        [JsonIgnore]
        public  ICollection<Dt_AppointmentService>? AppointmentServices { get; set; }
    }
}
