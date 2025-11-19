using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.WorkerDtos
{
    public class CreateWorkerDto
    {
        public string Name { get; set; }

        public int? ShopId { get; set; }
    }
}
