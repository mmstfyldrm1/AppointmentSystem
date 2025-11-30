using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.AppointmentDtos
{
    public class UpdateAppointmentServiceDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }

        public int ServiceId { get; set; }
    }
}
