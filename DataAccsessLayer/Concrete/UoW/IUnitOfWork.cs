using DataAccsessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Concrete.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository AppointmentRepository { get; }

        IAppointmentProductRepository AppointmentProductRepository { get; }

        IAppointmentServiceRepository AppointmentServiceRepository { get; }
        
        IApplicationRoleRepository ApplicationRoleRepository { get; }

        IApplicationUserRepository ApplicationUserRepository { get; }

        INotificationRepository NotificationRepository { get; }

        IPaymentRepository PaymentRepository { get; }   

        IProductRepository ProductRepository { get; }

        IProductCategoryRepository ProductCategoryRepository { get; }

        IReviewRepository ReviewRepository { get; } 

        IShopRepository ShopRepository { get; } 

        ITimeSlotRepository TimeSlotRepository { get; } 

        IWorkerRepository WorkerRepository { get; } 

        IOtherUserRepository OtherUserRepository { get; }   

        IServiceRepository ServiceRepository { get; }

        IDiscountRepository DiscountRepository { get; }

        IShopOwnerRepository ShopOwnerRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
