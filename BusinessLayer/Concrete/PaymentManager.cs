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
    public class PaymentManager : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_Payment t)
        {
            await _unitOfWork.PaymentRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_Payment t)
        {
            await _unitOfWork.PaymentRepository.TDelete(t);


        }

        public async Task<Dt_Payment> GetById(int id)
        {
            return await _unitOfWork.PaymentRepository.TGetById(id);

        }

        public async Task<List<Dt_Payment>> GetList()
        {
            return await _unitOfWork.PaymentRepository.TGetAll();
        }

        public async Task Update(Dt_Payment t)
        {
            await _unitOfWork.PaymentRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
