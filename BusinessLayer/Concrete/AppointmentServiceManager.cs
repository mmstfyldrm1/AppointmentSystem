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
    public class AppointmentServiceManager : IAppointmentserviceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentServiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_AppointmentService t)
        {
            await _unitOfWork.AppointmentServiceRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_AppointmentService t)
        {
            await _unitOfWork.AppointmentServiceRepository.TDelete(t);


        }

        public async Task<Dt_AppointmentService> GetById(int id)
        {
            return await _unitOfWork.AppointmentServiceRepository.TGetById(id);

        }

        public async Task<List<Dt_AppointmentService>> GetList()
        {
            return await _unitOfWork.AppointmentServiceRepository.TGetAll();
        }

        public async Task Update(Dt_AppointmentService t)
        {
            await _unitOfWork.AppointmentServiceRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
