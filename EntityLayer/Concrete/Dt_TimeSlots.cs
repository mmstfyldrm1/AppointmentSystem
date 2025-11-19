using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_TimeSlots
    {
        public int Id { get; set; }
        public string Slot { get; set; } // Örn: "09:00-09:30"
       
        [JsonIgnore]
        public  ICollection<Dt_Appointment>? Appointments { get; set; }
    }
}
