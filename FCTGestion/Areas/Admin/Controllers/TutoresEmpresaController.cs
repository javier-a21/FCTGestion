﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FCTGestion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TutoresEmpresaController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TutoresEmpresaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: TutorCentro/TutoresEmpresa
        public async Task<IActionResult> Index()
        {
            var tutores = await _context.TutoresEmpresa
                .Include(t => t.Empresa)
                .ToListAsync();

            return View(tutores);
        }

        // GET: TutorCentro/TutoresEmpresa/Create
        public IActionResult Create(int? empresaId)
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas.ToList(), "Id", "Nombre", empresaId);

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Correo,EmpresaId")] TutorEmpresa tutorEmpresa)
        {
            if (ModelState.IsValid)
            {
                // Crear usuario en Identity
                var user = new ApplicationUser
                {
                    UserName = tutorEmpresa.Correo,
                    Email = tutorEmpresa.Correo,
                    EmailConfirmed = true,
                    DebeCambiarPassword = true // si usas esto
                };

                // Contraseña por defecto
                var password = "TutorEmpresa123."; 

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    // Asignar rol
                    await _userManager.AddToRoleAsync(user, "TutorEmpresa");

                    // Vincular el tutor con el nuevo usuario
                    tutorEmpresa.UserId = user.Id;

                    _context.Add(tutorEmpresa);
                    await _context.SaveChangesAsync();
                    TempData["MensajeCrearEmpresa"] = "Tutor añadido correctamente.";
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

            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", tutorEmpresa.EmpresaId);
            return View(tutorEmpresa);
        }
        private bool TutorEmpresaExists(int id)
        {
            return _context.TutoresEmpresa.Any(e => e.Id == id);
        }


        // GET: TutorCentro/TutoresEmpresa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tutor = await _context.TutoresEmpresa.FindAsync(id);
            if (tutor == null) return NotFound();

            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", tutor.EmpresaId);
            return View(tutor);
        }

        // POST: TutorCentro/TutoresEmpresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Correo,EmpresaId")] TutorEmpresa tutorEmpresa)
        {
            if (id != tutorEmpresa.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutorEmpresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorEmpresaExists(tutorEmpresa.Id)) return NotFound();
                    else throw;
                }
                TempData["MensajeCrearEmpresa"] = "Tutor actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", tutorEmpresa.EmpresaId);
            return View(tutorEmpresa);
        }

        // GET: TutorCentro/TutoresEmpresa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tutor = await _context.TutoresEmpresa
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tutor == null) return NotFound();

            return View(tutor);
        }

        // GET: TutorCentro/TutoresEmpresa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tutor = await _context.TutoresEmpresa
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tutor == null) return NotFound();

            return View(tutor);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutorEmpresa = await _context.TutoresEmpresa.FindAsync(id);

            if (tutorEmpresa == null)
            {
                return NotFound();
            }

            // Desvincular Alumnos relacionados con este TutorEmpresa
            var alumnosRelacionados = _context.Alumnos.Where(a => a.TutorEmpresaId == id).ToList();
            foreach (var alumno in alumnosRelacionados)
            {
                alumno.TutorEmpresaId = null;
            }

            // Eliminar Contactos relacionados
            var contactosRelacionados = _context.Contactos.Where(c => c.TutorEmpresaId == id);
            _context.Contactos.RemoveRange(contactosRelacionados);

            // Eliminar TutorEmpresa
            _context.TutoresEmpresa.Remove(tutorEmpresa);

            await _context.SaveChangesAsync();
            TempData["MensajeCrearEmpresa"] = "Tutor de Empresa eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }


    }
}
