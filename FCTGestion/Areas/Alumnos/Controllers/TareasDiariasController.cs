using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FCTGestion.Data;
using FCTGestion.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace FCTGestion.Areas.Alumnos.Controllers
{
    [Area("Alumnos")]
    [Authorize(Roles = "Alumno")]
    public class TareasDiariasController : Controller
    {
      

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TareasDiariasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var tareas = await _context.TareasDiarias
                .Include(t => t.Alumno)
                .Where(t => t.Alumno.UserId == userId)
                .ToListAsync();
            return View(tareas);
        }
    }
}


