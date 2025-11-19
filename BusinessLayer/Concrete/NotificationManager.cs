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
    public class NotificationManager : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Dt_Notification t)
        {
            await _unitOfWork.NotificationRepository.TAdd(t);
            _unitOfWork.SaveChangesAsync();


        }

        public async Task Delete(Dt_Notification t)
        {
            await _unitOfWork.NotificationRepository.TDelete(t);


        }

        public async Task<Dt_Notification> GetById(int id)
        {
            return await _unitOfWork.NotificationRepository.TGetById(id);

        }

        public async Task<List<Dt_Notification>> GetList()
        {
            return await _unitOfWork.NotificationRepository.TGetAll();
        }

        public async Task Update(Dt_Notification t)
        {
            await _unitOfWork.NotificationRepository.TUpdate(t);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
