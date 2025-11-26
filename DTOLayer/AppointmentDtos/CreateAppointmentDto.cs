using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOLayer.AppointmentDtos
{
    public class CreateAppointmentDto
    {
       
       
        public DateTime AppointmentDate { get; set; }

        public int? ApplicationUserId { get; set; } //FK

        public string? Explanation { get; set; }

        public string? UserName { get; set; }

        public string? UserSurname { get; set; }

        public string? UserPhone { get; set; }

        public int WorkerId { get; set; }

        public int TimeSlotId { get; set; }

        public int AppointmentStatus { get; set; }

        public int ServicesId { get; set; }

        public int ShopId { get; set; }
        public string WorkerName { get; set; }

        public string ShopName { get; set; }

        public string TimeSlot { get; set; }


    }
}
