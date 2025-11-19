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
    public class ShopOwnerManager : IShopOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopOwnerManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_ShopOwner t)
        {
            await _unitOfWork.ShopOwnerRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_ShopOwner t)
        {
            await _unitOfWork.ShopOwnerRepository.TDelete(t);


        }

        public async Task<Dt_ShopOwner> GetById(int id)
        {
            return await _unitOfWork.ShopOwnerRepository.TGetById(id);

        }

        public async Task<List<Dt_ShopOwner>> GetList()
        {
            return await _unitOfWork.ShopOwnerRepository.TGetAll();
        }

        public async Task Update(Dt_ShopOwner t)
        {
            await _unitOfWork.ShopOwnerRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
