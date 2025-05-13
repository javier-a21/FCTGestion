using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using FCTGestion.Helpers;

namespace FCTGestion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AlumnosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlumnosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Alumnos
        public async Task<IActionResult> Index()
        {
            var alumnos = await _context.Alumnos
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



        public IActionResult Create()
        {
            ViewBag.TutorCentroId = new SelectList(_context.TutoresCentro, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,CorreoEducacion,SeguridadSocial,TutorCentroId")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
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

                        // Asignamos el UserId y TutorCentroId seleccionados
                        alumno.UserId = user.Id;

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

            ViewBag.TutoresCentro = new SelectList(_context.TutoresCentro, "Id", "Nombre");
            return View(alumno);
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

        // GET: Admin/Alumnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();

            ViewData["TutorCentroId"] = new SelectList(_context.TutoresCentro, "Id", "Nombre", alumno.TutorCentroId);
            return View(alumno);
        }

        // POST: Admin/Alumnos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,CorreoEducacion,SeguridadSocial,TutorCentroId")] Alumno alumno)
        {
            if (id != alumno.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumno);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Alumnos.Any(a => a.Id == alumno.Id))
                        return NotFound();
                    throw;
                }
            }

            ViewData["TutorCentroId"] = new SelectList(_context.TutoresCentro, "Id", "Nombre", alumno.TutorCentroId);
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
    }
    }
