// Areas/Patient/Controllers/PatientControllerBase.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST8002.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient")]
    [AutoValidateAntiforgeryToken]
    public abstract class PatientControllerBase : Controller { }
}
