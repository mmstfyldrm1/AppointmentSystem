using AutoMapper;
using DTOLayer.AppointmentDtos;
using DTOLayer.ServicesDtos;
using DTOLayer.ShopOwnersDtos;
using DTOLayer.ShopOwnersDtos.AddShopOwnerDtos;
using DTOLayer.WorkerDtos;
using EntityLayer.Concrete;

namespace AppointmentSystem.Mapping.AutoMapperProfile
{
	public class MapProfile : Profile
	{
		protected MapProfile()
		{
			CreateMap<Dt_Appointment, CreateAppointmentDto>();
			CreateMap<CreateAppointmentDto, Dt_Appointment>();

            CreateMap<Dt_ShopOwner, AddShopOwnersDto>();
            CreateMap<AddShopOwnersDto, Dt_ShopOwner>();

            CreateMap<Dt_ShopOwner, UpdateShopOwnersDto>();
            CreateMap<UpdateShopOwnersDto, Dt_ShopOwner>();

            CreateMap<Dt_Service, AdminPanelAddServicesDto>();
            CreateMap<AdminPanelAddServicesDto, Dt_Service>();

            CreateMap<Dt_AppointmentService, CreateAppointmentServicesDto>();
            CreateMap<CreateAppointmentServicesDto, Dt_AppointmentService>();

            CreateMap<Dt_Worker, CreateWorkerDto>();
            CreateMap<CreateWorkerDto, Dt_Worker>();

            CreateMap<Dt_Worker, UpdateWorkerDto>();
            CreateMap<UpdateWorkerDto, Dt_Worker>();

            CreateMap<Dt_Appointment, UpdateAppointmentDto>();
            CreateMap<UpdateAppointmentDto, Dt_Appointment>();

            CreateMap<Dt_AppointmentService, UpdateAppointmentServiceDto>();
            CreateMap<UpdateAppointmentServiceDto, Dt_AppointmentService>();

        }
	}
}
