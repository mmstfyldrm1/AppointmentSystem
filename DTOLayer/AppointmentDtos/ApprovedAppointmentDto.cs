using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.AppointmentDtos
{
    public class ApprovedAppointmentDto
    {
        public int AppointmentId { get; set; }

        public int ShopId { get; set; }

        public string UserName { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int IsClosed { get; set; }

        public string ShopName { get; set; }

        public string ServiceName { get; set; }

        public string WorkerName { get; set; }

        public decimal Price { get; set; }


        public string TimeSlot { get; set; }

        public int AppointmentStatus { get; set; }

    }
}
