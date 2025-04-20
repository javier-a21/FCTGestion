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

    // POST: Admin/TutoresCentro/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var tutor = await _context.TutoresCentro.FindAsync(id);
        if (tutor == null) return NotFound();

        _context.TutoresCentro.Remove(tutor);
        await _context.SaveChangesAsync();
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

                TempData["Mensaje"] = "Tutor creado correctamente";
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

}
