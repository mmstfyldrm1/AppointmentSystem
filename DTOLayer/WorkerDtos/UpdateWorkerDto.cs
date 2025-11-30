using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.WorkerDtos
{
    public class UpdateWorkerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Email { get; set; }

        public int ApplicationUserId { get; set; }

        public int? ShopId { get; set; }

        public string? WorkerImg { get; set; }

        public IFormFile? Image { get; set; }

        public string? WorkerPhone { get; set; }

        public DateTime? InsertedDate { get; set; }

        public DateTime? UpdateDate { get; set; } = DateTime.Now;

        public int? WorkerStatus { get; set; }
    }
}
