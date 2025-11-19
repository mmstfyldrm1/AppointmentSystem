using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public string? ApplicationUserId { get; set; } //FK
       
        [JsonIgnore]
        public  Dt_ApplicationUser? ApplicationUser { get; set; } // Navigation property
    }
}
