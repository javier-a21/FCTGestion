using System.Threading.Tasks;
using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FCTGestion.Areas.TutorCentro.Controllers
{
    [Area("TutorCentro")]
    [Authorize(Roles = "TutorCentro")]
    public class ContactoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContactoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ContactoController
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(a => a.UserId == userId);

            if (tutorCentro == null)
            {
                return NotFound();
            }

            var contactos = await _context.Contactos
                .Include(c => c.Alumno)
                .Include(c => c.TutorEmpresa)
                .Where(c => c.TutorCentroId == tutorCentro.Id)
                .ToListAsync();

            return View(contactos);
        }

        // GET: ContactoController/Create
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(t => t.UserId == userId);

            if (tutorCentro == null)
            {
                return NotFound();
            }

            // Cargar solo los alumnos asignados al TutorCentro
            ViewBag.Alumnos = await _context.Alumnos
                                           .Where(a => a.TutorCentroId == tutorCentro.Id)
                                           .ToListAsync();

            ViewBag.TutoresEmpresa = await _context.TutoresEmpresa.ToListAsync();

            return View();
        }


        // POST: ContactoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contacto contacto)
        {
            var userId = _userManager.GetUserId(User);
            var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(t => t.UserId == userId);

            if (tutorCentro == null)
            {
                return NotFound();
            }

            contacto.TutorCentroId = tutorCentro.Id;

            if (contacto.AlumnoId == null && contacto.TutorEmpresaId == null)
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un Alumno o un Tutor de Empresa.");
            }

            if (ModelState.IsValid)
            {
                _context.Contactos.Add(contacto);
                await _context.SaveChangesAsync();
                TempData["MensajeEliminacionContacto"] = "✅ Contacto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Recargar dropdowns si hay errores
            ViewBag.Alumnos = await _context.Alumnos.ToListAsync();
            ViewBag.TutoresEmpresa = await _context.TutoresEmpresa.ToListAsync();

            return View(contacto);
        }

        // GET: ContactoController/Edit/5


        public async Task<IActionResult> Edit(int id)
        {
            var contacto = await _context.Contactos.FindAsync(id);
            if (contacto == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(t => t.UserId == userId);

            if (tutorCentro == null)
            {
                return NotFound();
            }

            ViewBag.Alumnos = await _context.Alumnos
                                            .Where(a => a.TutorCentroId == tutorCentro.Id)
                                            .ToListAsync();

            ViewBag.TutoresEmpresa = await _context.TutoresEmpresa.ToListAsync();

            return View(contacto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contacto contacto)
        {
            if (id != contacto.Id)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(t => t.UserId == userId);

            if (tutorCentro == null)
            {
                ModelState.AddModelError("", "No se ha encontrado un TutorCentro asociado al usuario actual.");
                return View(contacto);
            }

            // Asignar el TutorCentroId del usuario logueado
            contacto.TutorCentroId = tutorCentro.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contacto);
                    await _context.SaveChangesAsync();
                    TempData["MensajeEliminacionContacto"] = "✅ Contacto actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", $"Error al actualizar el contacto: {errorMessage}");
                }
            }

            ViewBag.Alumnos = await _context.Alumnos
                                            .Where(a => a.TutorCentroId == tutorCentro.Id)
                                            .ToListAsync();
            ViewBag.TutoresEmpresa = await _context.TutoresEmpresa.ToListAsync();

            return View(contacto);
        }

        // GET: Contacto/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(t => t.UserId == userId);

            if (tutorCentro == null)
            {
                return NotFound();
            }

            var contacto = await _context.Contactos
                .Include(c => c.Alumno)
                .Include(c => c.TutorEmpresa)
                .FirstOrDefaultAsync(c => c.Id == id && c.TutorCentroId == tutorCentro.Id);

            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        // POST: Contacto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var tutorCentro = await _context.TutoresCentro.FirstOrDefaultAsync(t => t.UserId == userId);

            if (tutorCentro == null)
            {
                return NotFound();
            }

            var contacto = await _context.Contactos
                .FirstOrDefaultAsync(c => c.Id == id && c.TutorCentroId == tutorCentro.Id);

            if (contacto == null)
            {
                return NotFound();
            }

            try
            {
                _context.Contactos.Remove(contacto);
                await _context.SaveChangesAsync();
                TempData["MensajeEliminacionContacto"] = "✅ Contacto eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeEliminacionContacto"] = $"Error al eliminar el contacto: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
        }

    }
