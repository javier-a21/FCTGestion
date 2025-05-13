using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class TutoresCentroController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TutoresCentroController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Admin/TutoresCentro
    public async Task<IActionResult> Index()
    {
        var tutores = await _context.TutoresCentro.Include(t => t.Usuario).ToListAsync();
        return View(tutores);
    }

    // GET: Admin/TutoresCentro/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var tutor = await _context.TutoresCentro
                                  .FirstOrDefaultAsync(t => t.Id == id);

        if (tutor == null) return NotFound();

        return View(tutor);
    }

    // POST: Admin/TutoresCentro/DeleteConfirmed/5
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tutor = await _context.TutoresCentro.FindAsync(id);

        if (tutor == null)
        {
            return NotFound();
        }

        // Desvincular Alumnos relacionados con este TutorCentro
        var alumnosRelacionados = _context.Alumnos.Where(a => a.TutorCentroId == id).ToList();
        foreach (var alumno in alumnosRelacionados)
        {
            alumno.TutorCentroId = null;
        }

        // Guardar cambios antes de eliminar el TutorCentro
        await _context.SaveChangesAsync();

        // Eliminar TutorCentro
        _context.TutoresCentro.Remove(tutor);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Tutor Centro eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }


    // GET: Admin/TutoresCentro/Create
    public IActionResult Create()
    {
        ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Email");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Correo")] TutorCentro tutorCentro)
    {
        if (ModelState.IsValid)
        {
            // 1. Crear el ApplicationUser asociado
            var user = new ApplicationUser
            {
                UserName = tutorCentro.Correo,
                Email = tutorCentro.Correo,
                EmailConfirmed = true
            };

            var passwordPorDefecto = "TutorCentro123.";
            var result = await _userManager.CreateAsync(user, passwordPorDefecto);

            if (result.Succeeded)
            {
                // 2. Asignar rol
                await _userManager.AddToRoleAsync(user, "TutorCentro");

                // 3. Guardar el tutor con el UserId vinculado
                tutorCentro.UserId = user.Id;
                _context.TutoresCentro.Add(tutorCentro);
                await _context.SaveChangesAsync();

                TempData["MensajeCrearTutoresCentro"] = "Tutor Añadido correctamente.";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        return View(tutorCentro);
    }
    // GET: Admin/TutoresCentro/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var tutorCentro = await _context.TutoresCentro
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tutorCentro == null) return NotFound();

        return View(tutorCentro);
    }

    // POST: Admin/TutoresCentro/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Correo,UserId")] TutorCentro tutorCentro)
    {
        if (id != tutorCentro.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var tutorExistente = await _context.TutoresCentro
                    .Include(t => t.Usuario)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tutorExistente == null) return NotFound();

                tutorExistente.Nombre = tutorCentro.Nombre;
                tutorExistente.Correo = tutorCentro.Correo;

                // Actualizar también el ApplicationUser (usuario)
                if (tutorExistente.Usuario != null)
                {
                    tutorExistente.Usuario.Email = tutorCentro.Correo;
                    tutorExistente.Usuario.UserName = tutorCentro.Correo;
                    _context.Update(tutorExistente.Usuario);
                }

                _context.Update(tutorExistente);
                await _context.SaveChangesAsync();

                TempData["MensajeCrearTutoresCentro"] = "Tutor actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        return View(tutorCentro);
    }

}
