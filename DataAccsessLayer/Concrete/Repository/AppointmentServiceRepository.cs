using DataAccsessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Concrete.Repository
{
    public class AppointmentServiceRepository : GenericRepository<Dt_AppointmentService>, IAppointmentServiceRepository
    {
        public AppointmentServiceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
