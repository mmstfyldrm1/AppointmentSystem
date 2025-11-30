using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_Review
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // Örn: 1-5 arası puan

        public int? AppointmentId { get; set; }
        [JsonIgnore]
        public  Dt_Appointment? Appointment { get; set; }

        public int ApplicationUserId { get; set; } //FK
        [JsonIgnore]
        public  Dt_ApplicationUser? ApplicationUser { get; set; } //// Navigation property
    }
}
