using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.ServicesDtos
{
    public class AdminPanelServicesListDto
    {
        public string ServicesName { get; set; }

        public string Price { get; set; }

        public string Explanation { get; set; }

        public int ServicesType { get; set; }

        public string ShopName { get; set; }


    }


}
