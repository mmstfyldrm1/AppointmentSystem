using AutoMapper;
using DTOLayer.AppointmentDtos;
using EntityLayer.Concrete;

namespace AppointmentSystem.Mapping.AutoMapperProfile
{
	public class MapProfile : Profile
	{
		protected MapProfile()
		{
			CreateMap<Dt_Appointment, CreateAppointmentDto>();
			CreateMap<CreateAppointmentDto, Dt_Appointment>();

		}
	}
}
