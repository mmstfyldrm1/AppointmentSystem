using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.WorkerDtos
{
    public class AdminPanelWorkerDto
    {
        public string WorkerName { get; set; }

        public string? WorkerImg { get; set; }

       
        public string? WorkerPhone { get; set; }

        public string? ShopName { get; set; }

        public int? TotalAppointmentCount { get; set; }
        public int? TodayTotalAppointmentCount { get; set; }

        public int? Status { get; set; }

        public DateTime? InsertedDate { get;set; }



    }
}
