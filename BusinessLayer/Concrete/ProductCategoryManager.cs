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
    public class ProductCategoryManager : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_ProductCategory t)
        {
            await _unitOfWork.ProductCategoryRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_ProductCategory t)
        {
            await _unitOfWork.ProductCategoryRepository.TDelete(t);


        }

        public async Task<Dt_ProductCategory> GetById(int id)
        {
            return await _unitOfWork.ProductCategoryRepository.TGetById(id);

        }

        public async Task<List<Dt_ProductCategory>> GetList()
        {
            return await _unitOfWork.ProductCategoryRepository.TGetAll();
        }

        public async Task Update(Dt_ProductCategory t)
        {
            await _unitOfWork.ProductCategoryRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
