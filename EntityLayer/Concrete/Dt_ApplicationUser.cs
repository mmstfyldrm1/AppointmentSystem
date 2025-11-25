using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_ApplicationUser:IdentityUser<int>
    {
        public string? FullName { get; set; }


      




        // İlişkiler

        [JsonIgnore]
        public  ICollection<Dt_Appointment>? Appointments { get; set; }
        
        [JsonIgnore]
        public  ICollection<Dt_Review>? Reviews { get; set; }
        
        [JsonIgnore]
        public  ICollection<Dt_Notification>? Notifications { get; set; }
    }
}
