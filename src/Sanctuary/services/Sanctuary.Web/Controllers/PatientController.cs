using Microsoft.AspNetCore.Mvc;
using Sanctuary.Web.Models.Patient;

namespace Sanctuary.Web.Controllers
{
    public class PatientController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(Guid patientId) 
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetQuickRefPatients()
        {

            return PartialView(@"~/Views/Patient/partials/_Add.cshtml", new AddPatientDto());
        }

        [HttpGet]
        public IActionResult Add() 
        {
            
            return PartialView(@"~/Views/Patient/partials/_Add.cshtml", new AddPatientDto());
        }


    }
}
