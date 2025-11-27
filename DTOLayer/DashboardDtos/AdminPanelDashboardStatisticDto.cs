using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DashboardDtos
{
    public class AdminPanelDashboardStatisticDto
    {
        public int WorkerCount { get; set; }

        public int ServiceCount { get; set; }
        public int AppointmentCount { get; set; }

        public int ShopCount { get; set; }

        public int AppointmentCancelCount { get; set; }

    }
}
