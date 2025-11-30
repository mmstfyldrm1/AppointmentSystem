using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.ServicesDtos
{
    public class AdminPanelAddServicesDto
    {
       
        public string ServiceName { get; set; }

        public int ShopId { get; set; }
        public decimal Price { get; set; }

        public string Explanation { get; set; }


        public int ServicesType { get; set; }    


       
    }
}
