using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using FCTGestion.Helpers;

namespace FCTGestion.Areas.TutorCentro.Controllers
{
    [Area("TutorCentro")]
    [Authorize(Roles = "TutorCentro")]
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
            var userId = _userManager.GetUserId(User);

            var tutor = await _context.TutoresCentro.FirstOrDefaultAsync(a => a.UserId == userId);
            if (tutor == null) return NotFound();

            var alumnos = await _context.Alumnos
                .Where(a => a.TutorCentroId == tutor.Id)
                .Include(a => a.TutorCentro)
                .Include(a => a.Empresa)
                .Include(a => a.TutorEmpresa)
                .ToListAsync();

            return View(alumnos);
        }
        private string GenerarContrasenaSegura()
        {
            const string mayusculas = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string minusculas = "abcdefghijkmnopqrstuvwxyz";
            const string numeros = "23456789";
            const string simbolos = "!@#$%&*";
            const int longitudTotal = 10;

            var random = new Random();

            
            var caracteres = new List<char>
    {
        mayusculas[random.Next(mayusculas.Length)],
        minusculas[random.Next(minusculas.Length)],
        numeros[random.Next(numeros.Length)],
        simbolos[random.Next(simbolos.Length)]
    };

            
            string todos = mayusculas + minusculas + numeros + simbolos;
            while (caracteres.Count < longitudTotal)
            {
                caracteres.Add(todos[random.Next(todos.Length)]);
            }

            
            return new string(caracteres.OrderBy(x => random.Next()).ToArray());
        }


        // GET: TutorCentro/Alumnos/Create
        public IActionResult Create()
        {
            ViewData["TutorCentroId"] = new SelectList(_context.TutoresCentro, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,CorreoEducacion,SeguridadSocial")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(t => t.UserId == userId);

                if (tutorCentro == null)
                    return NotFound("No se encontró al tutor centro correspondiente.");

                try
                {
                    var contrasenaGenerada = GenerarContrasenaSegura();

                    var user = new ApplicationUser
                    {
                        UserName = Utilidades.NormalizarNombreUsuario(alumno.Nombre),
                        Email = alumno.CorreoEducacion,
                        EmailConfirmed = true,
                        DebeCambiarPassword = true
                    };

                    var result = await _userManager.CreateAsync(user, contrasenaGenerada);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Alumno");
                        //Se le añade al nuevo alumno: UserId y TutorCentroId
                        alumno.UserId = user.Id;
                        alumno.TutorCentroId = tutorCentro.Id;

                        _context.Alumnos.Add(alumno);
                        await _context.SaveChangesAsync();

                        // Pasar datos por TempData
                        TempData["CorreoAlumno"] = alumno.CorreoEducacion;
                        TempData["ContrasenaGenerada"] = contrasenaGenerada;

                        return RedirectToAction(nameof(ConfirmacionAlumno));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"ERROR: {ex.Message}");
                }
            }

            return View(alumno);
        }
        // GET: TutorCentro/Alumnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var alumno = await _context.Alumnos
                .Include(a => a.TutorCentro)
                .Include(a => a.Empresa)
                .Include(a => a.TutorEmpresa)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (alumno == null)
                return NotFound();

            return View(alumno);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.Id == id);

                if (alumno == null)
                    return NotFound();

                // Eliminar tareas diarias relacionadas
                var tareasRelacionadas = await _context.TareasDiarias
                    .Where(t => t.AlumnoId == id)
                    .ToListAsync();
                if (tareasRelacionadas.Any())
                    _context.TareasDiarias.RemoveRange(tareasRelacionadas);

                // Eliminar contactos relacionados
                var contactosRelacionados = await _context.Contactos
                    .Where(c => c.AlumnoId == id)
                    .ToListAsync();
                if (contactosRelacionados.Any())
                    _context.Contactos.RemoveRange(contactosRelacionados);

                // Eliminar usuario asociado
                if (!string.IsNullOrEmpty(alumno.UserId))
                {
                    var user = await _userManager.FindByIdAsync(alumno.UserId);
                    if (user != null)
                    {
                        await _userManager.DeleteAsync(user);
                    }
                }

                // Finalmente, eliminar el alumno
                _context.Alumnos.Remove(alumno);
                await _context.SaveChangesAsync();

                TempData["MensajeEliminacionAlumnos"] = "✅ Alumno y todas sus relaciones eliminadas correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["MensajeEliminacionAlumnos"] = $"❌ Error al eliminar el alumno: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }



        public IActionResult ConfirmacionAlumno()
        {
            if (TempData["CorreoAlumno"] == null || TempData["ContrasenaGenerada"] == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Correo = TempData["CorreoAlumno"];
            ViewBag.Contrasena = TempData["ContrasenaGenerada"];
            return View();
        }





        // GET: TutorCentro/Alumnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
                return NotFound();

            ViewData["TutorCentroId"] = new SelectList(_context.TutoresCentro, "Id", "Nombre", alumno.TutorCentroId);
            return View(alumno);
        }

        // POST: TutorCentro/Alumnos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,CorreoEducacion,SeguridadSocial,UserId,TutorCentroId")] Alumno alumno)
        {
            if (id != alumno.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Alumnos.Any(a => a.Id == alumno.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["TutorCentroId"] = new SelectList(_context.TutoresCentro, "Id", "Nombre", alumno.TutorCentroId);
            return View(alumno);
        }
        // GET: TutorCentro/Alumnos/AsignarEmpresa
        public async Task<IActionResult> AsignarEmpresa(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();

            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre");
            ViewData["TutorEmpresaId"] = new SelectList(_context.TutoresEmpresa, "Id", "Nombre");

            return View(alumno);
        }


        // POST: TutorCentro/Alumnos/AsignarEmpresa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarEmpresa(int idAlumno, int EmpresaId, int TutorEmpresaId, DateTime FechaInicio, DateTime FechaFin)
        {
            var alumno1 = await _context.Alumnos.FindAsync(idAlumno);
            var empresa1 = await _context.Empresas.FindAsync(EmpresaId);
            var tutor1 = await _context.TutoresEmpresa.FindAsync(TutorEmpresaId);

            if (alumno1 == null || empresa1 == null || tutor1 == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    alumno1.EmpresaId = EmpresaId;
                    alumno1.TutorEmpresaId = TutorEmpresaId;
                    alumno1.FechaInicioFCT = FechaInicio;
                    alumno1.FechaFinFCT = FechaFin;

                    await _context.SaveChangesAsync(); 

                    TempData["Mensaje"] = "✅ Empresa y tutor asignados correctamente.";

                    return RedirectToAction(nameof(Index));


                }
                catch (Exception ex)
                {
                    // Puedes poner un breakpoint aquí o un log
                    Console.WriteLine("Error al guardar: " + ex.Message);
                }
            }
            return RedirectToAction(nameof(Index));
    }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var empresa = await _context.Empresas
                .Include(e => e.Tutores)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (empresa == null) return NotFound();

            return View(empresa);
        }
        [HttpGet]
        public async Task<IActionResult> GetTutoresPorEmpresa(int empresaId)
        {
            var tutores = await _context.TutoresEmpresa
                .Where(t => t.EmpresaId == empresaId)
                .Select(t => new {
                    id = t.Id,
                    nombre = t.Nombre
                }).ToListAsync();

            return Json(tutores);
        }

    }

}
