using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOLayer.AppointmentDtos
{
    public class ResultSearchAppointmentDto
    {


        public int TimeSlotId { get; set; }

        public int WorkerId { get; set; }

        public int ShopId { get; set; }
        public string? Time { get; set; }
        public string? WorkerName { get; set; }
        public string? WorkerImg { get; set; }
        public string? ShopName { get; set; }
        public string? WorkerPhone { get; set; }

        public DateTime Date { get; set; }

        public string? ShopPhone { get; set; }
    }
}
