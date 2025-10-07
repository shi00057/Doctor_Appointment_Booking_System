using AutoMapper;
using CST8002.Application.DTOs;
using CST8002.Web.Areas.Admin.ViewModels.Users;
using CST8002.Web.Areas.Admin.ViewModels.Schedule;
using CST8002.Web.Areas.Doctor.ViewModels;
using CST8002.Web.Areas.Patient.ViewModels;
using CST8002.Web.ViewModels;

namespace CST8002.Web.Mapping
{
    public sealed class UiMappingProfile : Profile
    {
        public UiMappingProfile()
        {
            CreateMap<ScheduleSlotDto, SlotVm>();
            CreateMap<AppointmentDto, AppointmentVm>();

            CreateMap(typeof(PagedResult<>), typeof(PagedResultVm<>));

            CreateMap<DoctorProfileEditVm, DoctorDto>().ReverseMap();
            CreateMap<PatientProfileEditVm, PatientDto>().ReverseMap();

            CreateMap<GenerateSlotsVm, GenerateSlotsRequest>();
            CreateMap<GenerateSlotsRangeVm, GenerateSlotsRequest>()
                .ForMember(d => d.WorkDate, o => o.Ignore());

            CreateMap<PatientDto, PendingPatientItemVm>();

            CreateMap<NotificationDto, NotificationVm>();
        }
    }
}
