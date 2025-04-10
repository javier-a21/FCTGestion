using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Authorization;

namespace FCTGestion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TutorCentro/Empresas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empresas.ToListAsync());
        }

        // GET: TutorCentro/Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var empresa = await _context.Empresas
                .Include(e => e.Tutores) // 👈 Incluye tutores de empresa
                .FirstOrDefaultAsync(e => e.Id == id);

            if (empresa == null) return NotFound();

            return View(empresa);
        }


        // GET: TutorCentro/Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TutorCentro/Empresas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Telefono,Direccion")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: TutorCentro/Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null) return NotFound();

            return View(empresa);
        }

        // POST: TutorCentro/Empresas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Telefono,Direccion")] Empresa empresa)
        {
            if (id != empresa.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: TutorCentro/Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var empresa = await _context.Empresas.FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null) return NotFound();

            return View(empresa);
        }

        // POST: TutorCentro/Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.Id == id);
        }
    public async Task<IActionResult> AsignarTutores(int id)
        {
            var empresa = await _context.Empresas
                .Include(e => e.Tutores)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (empresa == null) return NotFound();

            ViewBag.TodosTutores = await _context.TutoresEmpresa.ToListAsync();
            return View(empresa);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarTutores(int id, List<int> tutoresSeleccionados)
        {
            var empresa = await _context.Empresas
                .Include(e => e.Tutores)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (empresa == null) return NotFound();

            var tutores = await _context.TutoresEmpresa
                .Where(t => tutoresSeleccionados.Contains(t.Id))
                .ToListAsync();

            empresa.Tutores = tutores;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }

    }
    }
