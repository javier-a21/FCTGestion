using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace FCTGestion.Areas.TutorCentro.Controllers
{
    [Area("TutorCentro")]
    [Authorize(Roles = "TutorCentro")]
    public class AlumnosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AlumnosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var alumnos = await _context.Alumnos
                .Include(a => a.TutorCentro)
                .ToListAsync();
            return View(alumnos);
        }
        // GET: TutorCentro/Alumnos/Create
        public IActionResult Create()
        {
            ViewData["TutorCentroId"] = new SelectList(_context.TutoresCentro, "Id", "Nombre");
            return View();
        }

        // POST: TutorCentro/Alumnos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,CorreoEducacion,SeguridadSocial,TutorCentroId")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Crear usuario de identidad
                    var user = new IdentityUser
                    {
                        UserName = alumno.CorreoEducacion,
                        Email = alumno.CorreoEducacion,
                        EmailConfirmed = true
                    };

                    string passwordPorDefecto = "Alumno123.";

                    var result = await _userManager.CreateAsync(user, passwordPorDefecto);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Alumno");

                        alumno.UserId = user.Id;
                        _context.Add(alumno);
                        await _context.SaveChangesAsync();

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
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"ERROR: {ex.Message}");
                }
            }

            ViewData["TutorCentroId"] = new SelectList(_context.TutoresCentro, "Id", "Nombre", alumno.TutorCentroId);
            return View(alumno);
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

        // POST: TutorCentro/Alumnos/AsignarEmpresa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarEmpresa(int idAlumno, int EmpresaId, int TutorEmpresaId)
        {
            var alumno1 = await _context.Alumnos.FindAsync(idAlumno);
            var empresa1 = await _context.Empresas.FindAsync(EmpresaId);
            
            if (alumno1 == null || empresa1 == null)
            {
                return NotFound();
            }
            else
            {

                var asignacion = new RAE
                {
                    AlumnoId = idAlumno,
                    EmpresaId = EmpresaId

                };

            }
            return View(alumno1);
    }
    }

}
