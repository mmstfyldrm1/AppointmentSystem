using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }

        public int? ApplicationUserId { get; set; } //FK

        public int  IsClosed { get; set; }

        public DateTime InsertadDate { get; set; } = DateTime.Now;

        public DateTime? UpdateDate { get; set; }

        public string? Explanation { get; set; }
        public string? UserName { get; set; }

        public string? UserSurname { get; set; }

        public string? UserPhone { get; set; }

        public int AppointmentStatus { get; set; } = 1;

        [JsonIgnore]
        public Dt_ApplicationUser? ApplicationUser { get; set; } // Navigation property

        public int? ShopId { get; set; }
       
        [JsonIgnore]
        public virtual Dt_Shop? Shop { get; set; }

        public int? WorkerId { get; set; }
       
        [JsonIgnore]
        public virtual Dt_Worker? Worker { get; set; }

        public int? TimeSlotId { get; set; }
       
        [JsonIgnore]
        public  Dt_TimeSlots? TimeSlot { get; set; }
       
        [JsonIgnore]
        public  ICollection<Dt_AppointmentService>? AppointmentServices { get; set; }
       
        [JsonIgnore]
        public  ICollection<Dt_AppointmentProduct>? AppointmentProducts { get; set; }
        
        [JsonIgnore]
        public  Dt_Payment? Payment { get; set; }
        
        [JsonIgnore]
        public  Dt_Discount? Discount { get; set; }

        [JsonIgnore]
        public  ICollection<Dt_Review>? Reviews { get; set; }
    }
}
