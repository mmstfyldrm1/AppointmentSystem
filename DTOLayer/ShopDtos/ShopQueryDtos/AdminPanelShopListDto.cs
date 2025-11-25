using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.ShopDtos.ShopQueryDtos
{
    public class AdminPanelShopListDto
    {
        public string Name { get; set; }

        public string ShopAddress { get; set; }

        public string ShopPhone { get; set; }

        public int Status { get; set; }

        public int TotalAppointmentCount { get; set; }

        public int TodayTotalAppointmentCount { get; set; } = DateTime.Now.Day;

        public int WorkerCount { get; set; }



    }
}
