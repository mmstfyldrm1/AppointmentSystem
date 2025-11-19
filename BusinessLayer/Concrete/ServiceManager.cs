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
    public class ServiceManager : IserviceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_Service t)
        {
            await _unitOfWork.ServiceRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_Service t)
        {
            await _unitOfWork.ServiceRepository.TDelete(t);


        }

        public async Task<Dt_Service> GetById(int id)
        {
            return await _unitOfWork.ServiceRepository.TGetById(id);

        }

        public async Task<List<Dt_Service>> GetList()
        {
            return await _unitOfWork.ServiceRepository.TGetAll();
        }

        public async Task Update(Dt_Service t)
        {
            await _unitOfWork.ServiceRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
