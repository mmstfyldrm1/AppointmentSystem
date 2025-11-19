using DataAccsessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Concrete.Repository
{
    public class ShopOwnerRepository : GenericRepository<Dt_ShopOwner>, IShopOwnerRepository
    {
        public ShopOwnerRepository(AppDbContext context) : base(context)
        {
        }
    }
}
