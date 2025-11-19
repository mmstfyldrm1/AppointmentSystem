using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class TimeSlotsMaanger : ITimeSlotService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TimeSlotsMaanger(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_TimeSlots t)
        {
            await _unitOfWork.TimeSlotRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_TimeSlots t)
        {
            await _unitOfWork.TimeSlotRepository.TDelete(t);


        }

        public async Task<Dt_TimeSlots> GetById(int id)
        {
            return await _unitOfWork.TimeSlotRepository.TGetById(id);

        }

        public async Task<List<Dt_TimeSlots>> GetList()
        {
            return await _unitOfWork.TimeSlotRepository.TGetAll();
        }

        public async Task Update(Dt_TimeSlots t)
        {
            await _unitOfWork.TimeSlotRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
