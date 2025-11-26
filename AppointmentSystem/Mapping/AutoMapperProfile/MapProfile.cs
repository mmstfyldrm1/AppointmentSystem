using AutoMapper;
using DTOLayer.AppointmentDtos;
using DTOLayer.ServicesDtos;
using DTOLayer.ShopOwnersDtos.AddShopOwnerDtos;
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

            CreateMap<Dt_Service, AdminPanelAddServicesDto>();
            CreateMap<AdminPanelAddServicesDto, Dt_Service>();

            CreateMap<Dt_AppointmentService, CreateAppointmentServicesDto>();
            CreateMap<CreateAppointmentServicesDto, Dt_AppointmentService>();

        }
	}
}
