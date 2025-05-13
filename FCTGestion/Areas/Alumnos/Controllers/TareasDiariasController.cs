using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace FCTGestion.Areas.Alumnos.Controllers
{
    [Area("Alumnos")]
    [Authorize(Roles = "Alumno")]
    public class TareasDiariasController : Controller
    {
      

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TareasDiariasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(DateTime? fechaReferencia)
        {
            var userId = _userManager.GetUserId(User);
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.UserId == userId);

            if (alumno == null) return NotFound();

            var fechaBase = fechaReferencia ?? DateTime.Today;
            var inicioSemana = fechaBase.AddDays(-(int)fechaBase.DayOfWeek + (fechaBase.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
            var finSemana = inicioSemana.AddDays(6);

            var tareas = await _context.TareasDiarias
                .Where(t => t.AlumnoId == alumno.Id && t.Fecha >= inicioSemana && t.Fecha <= finSemana)
                .OrderBy(t => t.Fecha)
                .ToListAsync();

            // Obtener la observación del TutorCentro para la semana
            var observacionSemana = tareas
                .Select(t => t.Observaciones)
                .FirstOrDefault(obs => !string.IsNullOrEmpty(obs)) ?? "Sin observaciones";

            ViewBag.FechaReferencia = fechaBase;
            ViewBag.SemanaInicio = inicioSemana.ToShortDateString();
            ViewBag.SemanaFin = finSemana.ToShortDateString();
            ViewBag.ObservacionSemana = observacionSemana;

            if (!tareas.Any())
            {
                TempData["Aviso"] = "No tienes tareas registradas esta semana.";
            }

            return View("Index", tareas);
        }
// GET: Alumnos/TareasDiarias/Create
public IActionResult Create()
        {
            return View();
        }

        // POST: Alumnos/TareasDiarias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fecha,Descripcion,Horas")] TareaDiaria tarea)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var alumno1 = await _context.Alumnos.FirstOrDefaultAsync(a => a.UserId == userId);

                if (alumno1 == null)
                {
                    return NotFound();
                }

                try
                {
                    var tareaInsert = new TareaDiaria
                    {
                        AlumnoId = alumno1.Id,
                        Fecha = tarea.Fecha,
                        Descripcion = tarea.Descripcion,
                        Horas = tarea.Horas
                    };
                    _context.TareasDiarias.Add(tareaInsert);
                    await _context.SaveChangesAsync();

                    TempData["MensajeTareaDiaria"] = "Tarea guardada correctamente ✅";
                    //  Redirigir a Index (listado)
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"ERROR: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            return View(tarea); 
        }

        [HttpGet]
        public async Task<IActionResult> getNumHoras()
        {
            var userId = _userManager.GetUserId(User);
            var alumno1 = await _context.Alumnos.FirstOrDefaultAsync(a => a.UserId == userId);
            if (alumno1 == null)
            {
                return NotFound();
            }

            var tareas = await _context.TareasDiarias.Where(t => t.AlumnoId == alumno1.Id).ToListAsync();
            var tareasHorasTotales = 0;

            foreach ( var tarea in tareas)
            {
                tareasHorasTotales += tarea.Horas;
            }

            return Json(tareasHorasTotales);
        }
        [HttpGet]
        public async Task<IActionResult> getInicioFin()
        {
            var userId = _userManager.GetUserId(User);
            var alumno1 = await _context.Alumnos.FirstOrDefaultAsync(a => a.UserId == userId);
            if (alumno1 == null)
            {
                return NotFound();
            }


            var fechaInicio = alumno1.FechaInicioFCT?.ToString("dd/MM/yyyy");
            var fechaFin = alumno1.FechaFinFCT?.ToString("dd/MM/yyyy");

            var fechas = new
            {
                Inicio = fechaInicio,
                Fin = fechaFin
            };

            return Json(fechas);
        }
        [HttpPost]
        public async Task<IActionResult> ValidarFecha([FromBody] DateTime fecha)
        {
            var userId = _userManager.GetUserId(User);
            var alumno1 = await _context.Alumnos.FirstOrDefaultAsync(a => a.UserId == userId);

            if (alumno1 == null)
            {
                return NotFound();
            }

            var yaExiste = await _context.TareasDiarias
        .AnyAsync(t => t.AlumnoId == alumno1.Id && t.Fecha.Date == fecha.Date);

            return Json(new { valido = yaExiste });
        }
        public async Task<IActionResult> Delete(DateTime fecha)
        {
            var userId = _userManager.GetUserId(User);
            var alumno1 = await _context.Alumnos.FirstOrDefaultAsync(a => a.UserId == userId);
            if (alumno1 == null)
            {
                return NotFound();
            }

            var tarea = await _context.TareasDiarias.FirstOrDefaultAsync(t => t.Fecha == fecha.Date && t.AlumnoId == alumno1.Id);
            if (tarea == null)
            {
                return NotFound();
            }
            _context.TareasDiarias.Remove(tarea);

            TempData["MensajeTareaDiaria"] = "Tarea eliminada correctamente ✅";

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}


