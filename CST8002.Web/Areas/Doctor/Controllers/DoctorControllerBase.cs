// Areas/Doctor/Controllers/DoctorControllerBase.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    [AutoValidateAntiforgeryToken]
    public abstract class DoctorControllerBase : Controller { }
}
