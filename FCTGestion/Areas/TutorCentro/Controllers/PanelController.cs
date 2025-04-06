using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCTGestion.Areas.TutorCentro.Controllers
{
    [Area("TutorCentro")]
    [Authorize(Roles = "TutorCentro")]
    public class PanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
