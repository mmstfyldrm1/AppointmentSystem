using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_AppointmentService
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
       
        [JsonIgnore]
        public  Dt_Appointment? Appointment { get; set; }

        public int ServiceId { get; set; }
        
        [JsonIgnore]
        public  Dt_Service? Service { get; set; }
    }
}
