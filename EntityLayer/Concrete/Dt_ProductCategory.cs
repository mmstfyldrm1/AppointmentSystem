using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_ProductCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
       
        [JsonIgnore]
        public  ICollection<Dt_Product>? Products { get; set; }
    }
}
