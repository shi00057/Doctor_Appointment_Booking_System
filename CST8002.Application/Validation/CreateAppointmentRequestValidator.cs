using System;
using CST8002.Application.DTOs;
using FluentValidation;

namespace CST8002.Application.Validation
{
    public sealed class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentRequestValidator()
        {
            RuleFor(x => x.DoctorId).GreaterThan(0);
            RuleFor(x => x.PatientId).GreaterThan(0);
            RuleFor(x => x.StartUtc).Must(x => x != default);
            RuleFor(x => x.EndUtc).Must(x => x != default);
            RuleFor(x => x).Must(x => x.StartUtc < x.EndUtc);
            RuleFor(x => x).Must(x =>
            {
                var span = x.EndUtc - x.StartUtc;
                return span.TotalMinutes > 0 && span.TotalMinutes <= TimeSpan.FromHours(4).TotalMinutes;
            });
        }
    }
}
