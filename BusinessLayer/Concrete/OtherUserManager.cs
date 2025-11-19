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
    public class OtherUserManager : IOtherUserService
    {
       private readonly IUnitOfWork _unitOfWork;

        public OtherUserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_OtherUser t)
        {
           await _unitOfWork.OtherUserRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public  async Task Delete(Dt_OtherUser t)
        {
           await _unitOfWork.OtherUserRepository.TDelete(t);    


        }

        public async Task<Dt_OtherUser> GetById(int id)
        {
            return await _unitOfWork.OtherUserRepository.TGetById(id);

        }

        public async Task<List<Dt_OtherUser>> GetList()
        {
            return await _unitOfWork.OtherUserRepository.TGetAll();
        }

        public async Task Update(Dt_OtherUser t)
        {
           await _unitOfWork.OtherUserRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
