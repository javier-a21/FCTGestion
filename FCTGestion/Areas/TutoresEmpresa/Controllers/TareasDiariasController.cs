using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FCTGestion.Areas.TutoresEmpresa.Controllers
{
    [Area("TutoresEmpresa")]
    [Authorize(Roles = "TutorEmpresa")]
    public class TareasDiariasController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TareasDiariasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var tutor = await _context.TutoresEmpresa.FirstOrDefaultAsync(t => t.UserId == userId);
            if (tutor == null) return NotFound();

            var alumnos = await _context.Alumnos
                .Where(a => a.TutorEmpresaId == tutor.Id)
                .ToListAsync();

            ViewBag.Alumnos = new SelectList(alumnos, "Id", "Nombre");

            return View();
        }



        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> ValidarSemana(int alumnoId, DateTime? fechaReferencia)
        {
            var userId = _userManager.GetUserId(User);

            var tutorEmpresa = await _context.TutoresEmpresa.FirstOrDefaultAsync(te => te.UserId == userId);
            if (tutorEmpresa == null)
                return NotFound();

            var alumno = await _context.Alumnos
                .Include(a => a.TutorEmpresa)
                .FirstOrDefaultAsync(a => a.Id == alumnoId && a.TutorEmpresaId == tutorEmpresa.Id);

            if (alumno == null)
                return NotFound("No tienes permiso para validar este alumno.");

            var fechaBase = fechaReferencia ?? DateTime.Today;
            var inicioSemana = fechaBase.AddDays(-(int)fechaBase.DayOfWeek + 1);
            var finSemana = inicioSemana.AddDays(6);

            var tareas = await _context.TareasDiarias
                .Where(t => t.AlumnoId == alumno.Id && t.Fecha >= inicioSemana && t.Fecha <= finSemana)
                .OrderBy(t => t.Fecha)
                .ToListAsync();

            // Obtener una única observación para la semana
            string observacionSemana = tareas.FirstOrDefault()?.Observaciones ?? "Sin observaciones";

            ViewBag.Alumno = alumno;
            ViewBag.SemanaInicio = inicioSemana;
            ViewBag.SemanaFin = finSemana;
            ViewBag.AlumnoId = alumnoId;
            ViewBag.FechaReferencia = fechaBase;
            ViewBag.ObservacionSemana = observacionSemana;

            return View(tareas);
        }

        [HttpPost]
        public async Task<IActionResult> ValidarSemana(int alumnoId, DateTime fechaInicio, DateTime fechaFin)
        {
            var userId = _userManager.GetUserId(User);

            var tutorEmpresa = await _context.TutoresEmpresa.FirstOrDefaultAsync(te => te.UserId == userId);
            if (tutorEmpresa == null)
                return Unauthorized();

            var tareas = await _context.TareasDiarias
                .Where(t => t.AlumnoId == alumnoId && t.Fecha >= fechaInicio && t.Fecha <= fechaFin)
                .ToListAsync();

            foreach (var tarea in tareas)
            {
                tarea.Estado = EstadoValidacion.Aprobada;
                tarea.FechaValidacion = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            TempData["MensajeValidacionTareas"] = "✅ Tareas validadas correctamente.";
            return RedirectToAction("ValidarSemana", new { alumnoId = alumnoId, fechaReferencia = fechaInicio });
        }


        [HttpPost]
        public async Task<IActionResult> GuardarObservacion(int alumnoId, DateTime fechaInicio, DateTime fechaFin, string observacion)
        {
            var userId = _userManager.GetUserId(User);
            var tutorEmpresa = await _context.TutoresEmpresa.FirstOrDefaultAsync(te => te.UserId == userId);
            if (tutorEmpresa == null)
                return Unauthorized();

            var tareas = await _context.TareasDiarias
                .Where(t => t.AlumnoId == alumnoId && t.Fecha >= fechaInicio && t.Fecha <= fechaFin)
                .ToListAsync();

            if (!tareas.Any())
            {
                TempData["MensajeValidacionTareas"] = "⚠️ No hay tareas registradas en esta semana.";
                return RedirectToAction("ValidarSemana", new { alumnoId, fechaReferencia = fechaInicio });
            }

            try
            {
                foreach (var tarea in tareas)
                {
                    tarea.Observaciones = observacion;
                }

                await _context.SaveChangesAsync();
                TempData["MensajeValidacionTareas"] = "💬 Observación guardada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeValidacionTareas"] = $"❌ Error al guardar la observación: {ex.Message}";
            }

            return RedirectToAction("ValidarSemana", new { alumnoId, fechaReferencia = fechaInicio });
        }

    }
}
