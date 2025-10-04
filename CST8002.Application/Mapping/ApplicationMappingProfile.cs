using AutoMapper;
using CST8002.Application.DTOs;
using CST8002.Application.Domain.Entities;

namespace CST8002.Application.Mapping
{
    public sealed class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<ScheduleSlot, ScheduleSlotDto>().ReverseMap();
        }
    }
}
