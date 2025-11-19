using DataAccsessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Concrete.Repository
{
    public class AppointmentProductRepository : GenericRepository<Dt_AppointmentProduct>, IAppointmentProductRepository
    {
        public AppointmentProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
