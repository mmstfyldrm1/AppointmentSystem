using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.AppointmentDtos
{




    public class GetAllAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }

        public string  MemberName { get; set; }

        public string WorkerName { get; set; }

        public string Time { get; set; }

        public string ServiceName { get; set; }
        public int AppointmentStatus { get; set; }

        public int IsClosed { get; set; }

        public DateTime? InsertedDate { get; set; }
       
        public DateTime? UpdateDate { get; set; }

    }
}
