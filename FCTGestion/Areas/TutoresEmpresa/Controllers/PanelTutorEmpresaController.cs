using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCTGestion.Areas.TutoresEmpresa.Controllers
{
    [Area("TutoresEmpresa")]
    [Authorize(Roles = "TutorEmpresa")] 
    public class PanelTutorEmpresaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    }