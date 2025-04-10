using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Authorization;

namespace FCTGestion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TutoresEmpresaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TutoresEmpresaController(ApplicationDbContext context)
        {
            _context = context;
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


        // POST: TutorCentro/TutoresEmpresa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Correo,EmpresaId")] TutorEmpresa tutorEmpresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutorEmpresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Nombre", tutorEmpresa.EmpresaId);
            return View(tutorEmpresa);
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

        // POST: TutorCentro/TutoresEmpresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutor = await _context.TutoresEmpresa.FindAsync(id);
            if (tutor != null)
            {
                _context.TutoresEmpresa.Remove(tutor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TutorEmpresaExists(int id)
        {
            return _context.TutoresEmpresa.Any(t => t.Id == id);
        }
    }
}
