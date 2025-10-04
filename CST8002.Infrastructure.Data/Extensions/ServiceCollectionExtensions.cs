using CST8002.Application.Interfaces.Repositories;
using CST8002.Infrastructure.Data.Configuration;
using CST8002.Infrastructure.Data.Connection;
using CST8002.Infrastructure.Data.Repositories;
using CST8002.Infrastructure.Data.Transactions;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CST8002.Infrastructure.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureData(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionName = "Database")
        {
            services.AddOptions<DbOptions>()
                    .Bind(configuration.GetSection(configSectionName))
                    .PostConfigure(o => o.Validate());

            services.PostConfigure<DbOptions>(o =>
            {
                DefaultTypeMap.MatchNamesWithUnderscores = o.MatchNamesWithUnderscores;
            });

            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, SqlUnitOfWork>();

            return services;
        }
    }
}
