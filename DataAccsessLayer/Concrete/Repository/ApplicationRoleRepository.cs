using DataAccsessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Concrete.Repository
{
    public class ApplicationRoleRepository : GenericRepository<Dt_ApplicationRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
