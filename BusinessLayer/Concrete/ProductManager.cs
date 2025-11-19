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
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_Product t)
        {
            await _unitOfWork.ProductRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_Product t)
        {
            await _unitOfWork.ProductRepository.TDelete(t);


        }

        public async Task<Dt_Product> GetById(int id)
        {
            return await _unitOfWork.ProductRepository.TGetById(id);

        }

        public async Task<List<Dt_Product>> GetList()
        {
            return await _unitOfWork.ProductRepository.TGetAll();
        }

        public async Task Update(Dt_Product t)
        {
            await _unitOfWork.ProductRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
