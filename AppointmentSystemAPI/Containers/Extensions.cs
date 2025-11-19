

using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccsessLayer.Abstract;
using DataAccsessLayer.Concrete.Repository;
using System.Runtime.CompilerServices;

namespace ApiLayer.Containers
{
    public static class Extensions
    {
        public static void ContainerDependencies(this IServiceCollection Services)
        {
            Services.AddScoped<IAppointmentService, AppointmentManager>();
            Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            Services.AddScoped<IAppointmentserviceService, AppointmentServiceManager>();
            Services.AddScoped<IAppointmentServiceRepository, AppointmentServiceRepository>();

            Services.AddScoped<IAppointmentProductService, AppointmentProductManager>();
            Services.AddScoped<IAppointmentProductRepository, AppointmentProductRepository>();

            Services.AddScoped<IDiscountService, DiscountManager>();
            Services.AddScoped<IDiscountRepository, DiscountRepository>();

            Services.AddScoped<INotificationService, NotificationManager>();
            Services.AddScoped<INotificationRepository, NotificationRepository>();

            Services.AddScoped<IOtherUserService, OtherUserManager>();
            Services.AddScoped<IOtherUserRepository, OtherUserRepository>();

            Services.AddScoped<IPaymentService, PaymentManager>();
            Services.AddScoped<IPaymentRepository, PaymentRepository>();

            Services.AddScoped<IProductCategoryService, ProductCategoryManager>();
            Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

            Services.AddScoped<IProductService, ProductManager>();
            Services.AddScoped<IProductRepository, ProductRepository>();

            Services.AddScoped<IReviewService, ReviewManager>();
            Services.AddScoped<IReviewRepository, ReviewRepository>();

            Services.AddScoped<IserviceService, ServiceManager>();
            Services.AddScoped<IServiceRepository, ServiceRepository>();

            Services.AddScoped<IShopOwnerService, ShopOwnerManager>();
            Services.AddScoped<IShopOwnerRepository, ShopOwnerRepository>();

            Services.AddScoped<IShopService, ShopManager>();
            Services.AddScoped<IShopRepository, ShopRepository>();

            Services.AddScoped<ITimeSlotService, TimeSlotsMaanger>();
            Services.AddScoped<ITimeSlotRepository, TimeSlotsRepository>();

            Services.AddScoped<IWorkerService, WorkerManager>();
            Services.AddScoped<IWorkerRepository, WorkerRepository>();


        }
    }
}
