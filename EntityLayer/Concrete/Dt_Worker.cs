using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string WorkerPhone { get; set; }

        public string? WorkerImg { get; set; }
        public int? ShopId { get; set; }
        
        [JsonIgnore]
        public  Dt_Shop? Shop { get; set; }
        
        [JsonIgnore]
        public  ICollection<Dt_Appointment>? Appointments { get; set; }
    }
}
