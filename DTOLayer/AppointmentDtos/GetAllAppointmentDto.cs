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

        public int ApplicationUserId { get; set; } //FK
    }
}
