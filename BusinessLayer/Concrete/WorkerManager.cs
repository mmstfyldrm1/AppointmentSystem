using BusinessLayer.Abstract;
using DataAccsessLayer.Abstract;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class WorkerManager : IWorkerService
    {
       private readonly IUnitOfWork _unitOfWork;

        public WorkerManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

     
        public async Task Add(Dt_Worker t)
        {
            await _unitOfWork.WorkerRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_Worker t)
        {
            await _unitOfWork.WorkerRepository.TDelete(t);


        }

        public async Task<Dt_Worker> GetById(int id)
        {
            return await _unitOfWork.WorkerRepository.TGetById(id);

        }

        public async Task<List<Dt_Worker>> GetList()
        {
            return await _unitOfWork.WorkerRepository.TGetAll();
        }

        public async Task Update(Dt_Worker t)
        {
            await _unitOfWork.WorkerRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
