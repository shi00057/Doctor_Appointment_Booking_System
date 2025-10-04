using CST8002.Application.Abstractions;
using CST8002.Application.Interfaces.Services;
using CST8002.Application.Services;
using CST8002.Infrastructure.Data.Extensions;
using CST8002.Web.Filters;
using CST8002.Web.Infrastructure;
using CST8002.Web.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(o =>
{
    o.Filters.Add<ExceptionHandlingFilter>();
    o.Filters.Add<ValidateModelFilter>();
}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

builder.Services.AddInfrastructureData(builder.Configuration);

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();

builder.Services.AddAutoMapper(
    typeof(CST8002.Application.Mapping.ApplicationMappingProfile),
    typeof(UiMappingProfile));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, HttpCurrentUser>();
builder.Services.AddSingleton<IDateTimeProvider, SystemClock>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Account/Login";
        o.LogoutPath = "/Account/Logout";
        o.ExpireTimeSpan = TimeSpan.FromHours(8);
        o.SlidingExpiration = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "root",
    pattern: "",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
