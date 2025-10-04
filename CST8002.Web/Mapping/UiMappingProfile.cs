using AutoMapper;
using CST8002.Application.Domain.Entities;
using CST8002.Application.DTOs;
using CST8002.Web.ViewModels;

namespace CST8002.Web.Mapping
{
    public sealed class UiMappingProfile : Profile
    {
        public UiMappingProfile()
        {
            CreateMap<ScheduleSlotDto, SlotVm>();
            CreateMap<AppointmentDto, AppointmentVm>();
            CreateMap<PatientDto, PatientVm>();
            CreateMap<DoctorDto, DoctorVm>();
            CreateMap(typeof(PagedResult<>), typeof(PagedResultVm<>));
        }
    }
}
