using System.Threading;
using FCTGestion.Data;
using FCTGestion.Helpers;
using FCTGestion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

            var contactos = await _context.Contactos.Where(c => c.TutorCentroId == tutorCentro.Id).ToListAsync();
            return View("Index", contactos);
        }

        // GET: ContactoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContactoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fecha,ConQuien,Como,Resumen")] Contacto contacto)
        {
            var userId = _userManager.GetUserId(User);
            var tutor = await _context.TutoresCentro.FirstOrDefaultAsync(t => t.UserId == userId);
            if (tutor == null)
            {
                return NotFound();
            }

            contacto.TutorCentroId = tutor.Id;

            ModelState.Clear(); 
            TryValidateModel(contacto); 

            if (ModelState.IsValid)
            {
                _context.Add(contacto);
                await _context.SaveChangesAsync();
                TempData["MensajeCreacionController"] = "✅ Contacto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(contacto);
        }


        // GET: ContactoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContactoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContactoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContactoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
