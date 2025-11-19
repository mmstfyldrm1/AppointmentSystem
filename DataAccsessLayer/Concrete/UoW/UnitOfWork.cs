using DataAccsessLayer.Abstract;
using DataAccsessLayer.Concrete.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Concrete.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

      
        public IAppointmentRepository AppointmentRepository => new AppointmentRepository(_context);

        public IAppointmentProductRepository AppointmentProductRepository =>  new AppointmentProductRepository(_context);

        public IAppointmentServiceRepository AppointmentServiceRepository =>  new AppointmentServiceRepository(_context);

        public IApplicationRoleRepository ApplicationRoleRepository =>  new ApplicationRoleRepository(_context);

        public IApplicationUserRepository ApplicationUserRepository => new ApplicationUserRepository(_context);

        public INotificationRepository NotificationRepository =>  new NotificationRepository(_context);

        public IPaymentRepository PaymentRepository =>  new PaymentRepository(_context);

        public IProductRepository ProductRepository => new ProductRepository(_context);

        public IProductCategoryRepository ProductCategoryRepository => new ProductCategoryRepository(_context);

        public IReviewRepository ReviewRepository => new ReviewRepository(_context);

        public IShopRepository ShopRepository => new ShopRepository(_context);

        public ITimeSlotRepository TimeSlotRepository => new TimeSlotsRepository(_context);

        public IWorkerRepository WorkerRepository => new WorkerRepository(_context);

        public IOtherUserRepository OtherUserRepository => new OtherUserRepository(_context);

        public IServiceRepository ServiceRepository => new ServiceRepository(_context);

        public IDiscountRepository DiscountRepository =>  new DiscountRepository(_context);

        public IShopOwnerRepository ShopOwnerRepository => new ShopOwnerRepository(_context);

      

        public async  Task<int> SaveChangesAsync()
        {
            try
            {
                _context.SaveChanges();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return 0;
           
        }

        public void Dispose() => _context.Dispose();
    }
}
