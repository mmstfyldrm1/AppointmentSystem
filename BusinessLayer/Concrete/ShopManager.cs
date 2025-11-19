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
    public class ShopManager : IShopService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_Shop t)
        {
            await _unitOfWork.ShopRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_Shop t)
        {
            await _unitOfWork.ShopRepository.TDelete(t);


        }

        public async Task<Dt_Shop> GetById(int id)
        {
            return await _unitOfWork.ShopRepository.TGetById(id);

        }

        public async Task<List<Dt_Shop>> GetList()
        {
            return await _unitOfWork.ShopRepository.TGetAll();
        }

        public async Task Update(Dt_Shop t)
        {
            await _unitOfWork.ShopRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
