using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FCTGestion.Data;

namespace Proyecto.Areas.TutoresEmpresa.Controllers
{
    [Area("TutoresEmpresa")]
    [Authorize(Roles = "TutorEmpresa")]
    public class AlumnosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlumnosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Obtenemos el tutorEmpresa vinculado al usuario
            var tutorEmpresa = await _context.TutoresEmpresa
                .FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (tutorEmpresa == null)
            {
                return View("SinAsignaciones");
            }

            // Cargamos los alumnos vinculados al tutor
            var alumnos = await _context.Alumnos
                .Include(a => a.Empresa)
                .Where(a => a.TutorEmpresaId == tutorEmpresa.Id)
                .ToListAsync();

            return View(alumnos);
        }
    }
}
