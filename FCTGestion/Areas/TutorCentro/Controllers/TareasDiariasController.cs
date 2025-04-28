using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FCTGestion.Areas.TutorCentro.Controllers
{
    [Area("TutorCentro")]
    [Authorize(Roles = "TutorCentro")]
    public class TareasDiariasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TareasDiariasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TutorCentro/TareasDiarias
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(tc => tc.UserId == userId);

            if (tutorCentro == null) return NotFound();

            var alumnos = await _context.Alumnos
                .Where(a => a.TutorCentroId == tutorCentro.Id)
                .ToListAsync();

            ViewBag.Alumnos = new SelectList(alumnos, "Id", "Nombre");

            return View();
        }

        // GET: TutorCentro/TareasDiarias/ConsultarSemana
        [HttpGet]
        public async Task<IActionResult> ConsultarSemana(int alumnoId, DateTime? fechaReferencia)
        {
            var userId = _userManager.GetUserId(User);
            var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(tc => tc.UserId == userId);

            if (tutorCentro == null) return NotFound();

            var alumno = await _context.Alumnos
                .FirstOrDefaultAsync(a => a.Id == alumnoId && a.TutorCentroId == tutorCentro.Id);

            if (alumno == null) return NotFound("No tienes permiso para ver este alumno.");

            var fechaBase = fechaReferencia ?? DateTime.Today;
            var inicioSemana = fechaBase.AddDays(-(int)fechaBase.DayOfWeek + 1); // lunes
            var finSemana = inicioSemana.AddDays(6); // domingo

            var tareas = await _context.TareasDiarias
                .Where(t => t.AlumnoId == alumnoId && t.Fecha >= inicioSemana && t.Fecha <= finSemana)
                .OrderBy(t => t.Fecha)
                .ToListAsync();

            ViewBag.Alumno = alumno;
            ViewBag.SemanaInicio = inicioSemana;
            ViewBag.SemanaFin = finSemana;
            ViewBag.AlumnoId = alumnoId;
            ViewBag.FechaReferencia = fechaBase;

            if (!tareas.Any())
            {
                TempData["Aviso"] = "Este alumno no tiene tareas registradas esta semana.";
            }

            return View(tareas);
        }
    }
}
