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
    public class DiscountManager : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_Discount t)
        {
            await _unitOfWork.DiscountRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_Discount t)
        {
            await _unitOfWork.DiscountRepository.TDelete(t);


        }

        public async Task<Dt_Discount> GetById(int id)
        {
            return await _unitOfWork.DiscountRepository.TGetById(id);

        }

        public async Task<List<Dt_Discount>> GetList()
        {
            return await _unitOfWork.DiscountRepository.TGetAll();
        }

        public async Task Update(Dt_Discount t)
        {
            await _unitOfWork.DiscountRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
