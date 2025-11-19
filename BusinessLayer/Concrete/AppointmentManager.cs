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
    public class AppointmentManager : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_Appointment t)
        {
            await _unitOfWork.AppointmentRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_Appointment t)
        {
            await _unitOfWork.AppointmentRepository.TDelete(t);


        }

        public async Task<Dt_Appointment> GetById(int id)
        {
            return await _unitOfWork.AppointmentRepository.TGetById(id);

        }

        public async Task<List<Dt_Appointment>> GetList()
        {
            return await _unitOfWork.AppointmentRepository.TGetAll();
        }

        public async Task Update(Dt_Appointment t)
        {
            await _unitOfWork.AppointmentRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
