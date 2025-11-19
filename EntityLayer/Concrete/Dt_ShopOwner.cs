using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Dt_ShopOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? ShopOwnerImg { get; set; }

        // Bir işletmenin bir sahibi olur
        [JsonIgnore]
        public  Dt_ApplicationUser? ApplicationUser { get; set; } // Navigation property
        public int  ApplicationUserId { get; set; } // FK
        
        [JsonIgnore]
        public  ICollection<Dt_Shop>? Shops { get; set; }
    }
}
