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
    public class AppointmentProductManager : IAppointmentProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_AppointmentProduct t)
        {
            await _unitOfWork.AppointmentProductRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_AppointmentProduct t)
        {
            await _unitOfWork.AppointmentProductRepository.TDelete(t);


        }

        public async Task<Dt_AppointmentProduct> GetById(int id)
        {
            return await _unitOfWork.AppointmentProductRepository.TGetById(id);

        }

        public async Task<List<Dt_AppointmentProduct>> GetList()
        {
            return await _unitOfWork.AppointmentProductRepository.TGetAll();
        }

        public async Task Update(Dt_AppointmentProduct t)
        {
            await _unitOfWork.AppointmentProductRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
