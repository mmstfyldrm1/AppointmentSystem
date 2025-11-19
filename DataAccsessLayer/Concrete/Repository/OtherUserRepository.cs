using DataAccsessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Concrete.Repository
{
    public class OtherUserRepository : GenericRepository<Dt_OtherUser>, IOtherUserRepository
    {
        public OtherUserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
