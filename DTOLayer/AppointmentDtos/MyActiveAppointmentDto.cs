using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.AppointmentDtos
{
    public class MyActiveAppointmentDto
    {

        public int? AppointmentId { get; set; }

        public int? WorkerId { get; set; }

        public int? TimeSlotId { get; set; }
        public int? ShopId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string? Time { get; set; }
        public string? WorkerName { get; set; }
        public string? WorkerImg { get; set; }
        public string? ShopName { get; set; }
        public string? WorkerPhone { get; set; }

        public int? AppintmentStatus { get; set; }

        public string? ShopPhone { get; set; }
    }
}
