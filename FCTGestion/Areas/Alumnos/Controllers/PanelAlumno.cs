using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FCTGestion.Areas.Alumnos.Controllers
{
    [Area("Alumnos")]
    [Authorize(Roles = "Alumno")]
    public class PanelAlumno : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
