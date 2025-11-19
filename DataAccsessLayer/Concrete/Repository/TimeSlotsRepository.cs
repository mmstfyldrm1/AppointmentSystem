using DataAccsessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Concrete.Repository
{
    public class TimeSlotsRepository : GenericRepository<Dt_TimeSlots>, ITimeSlotRepository
    {
        public TimeSlotsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
