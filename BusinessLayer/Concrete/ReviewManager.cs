using BusinessLayer.Abstract;
using DataAccsessLayer.Concrete.Repository;
using DataAccsessLayer.Concrete.UoW;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ReviewManager : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_Review t)
        {
            await _unitOfWork.ReviewRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_Review t)
        {
            await _unitOfWork.ReviewRepository.TDelete(t);


        }

        public async Task<Dt_Review> GetById(int id)
        {
            return await _unitOfWork.ReviewRepository.TGetById(id);

        }

        public async Task<List<Dt_Review>> GetList()
        {
            return await _unitOfWork.ReviewRepository.TGetAll();
        }

        public async Task Update(Dt_Review t)
        {
            await _unitOfWork.ReviewRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
